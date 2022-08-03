using System;
using UnityEngine;

// 컨트롤러의 Input 정의부
public enum ControlInputType { Pitch, Yaw, Roll, Flap }

public class AeroSurface : MonoBehaviour
{
    // 외부로 부터 물리 정보를 받아온다.
    //날개 구성파일 정보를 불러오는 부분
    [SerializeField] AeroSurfaceConfig config = null;
    // 땅에서 컨트롤을 하는가
    public bool IsControlSurface;
    
    public ControlInputType InputType;
    // Input 값을 배가 시킨다.
    public float InputMultiplyer = 1;
    // 플랩 각도
    private float flapAngle;
    // 플랩각도 제한하는 함수(-50도~50도 (라디안))
    public void SetFlapAngle(float angle)
    {
        flapAngle = Mathf.Clamp(angle, -Mathf.Deg2Rad * 50, Mathf.Deg2Rad * 50);
    }
    // 항공기 추력 계산 함수 (파라미터: 공기 속도, 공기 밀도, 상대위치)
    public BiVector3 CalculateForces(Vector3 worldAirVelocity, float airDensity, Vector3 relativePosition)
    {
       
        //force와 torque 값 선언
        BiVector3 forceAndTorque = new BiVector3();
        // 만약 게임 오브젝트가 없거나 날개 정보가 없을 때 forceAndTorque 출력
        if (!gameObject.activeInHierarchy || config == null) return forceAndTorque;

        // Accounting for aspect ratio effect on lift coefficient.
        // Cl 계수를 얻기 위한 공식
        float correctedLiftSlope = config.liftSlope * config.aspectRatio /
           (config.aspectRatio + 2 * (config.aspectRatio + 4) / (config.aspectRatio + 2));

        // Calculating flap deflection influence on zero lift angle of attack
        // and angles at which stall happens.

        // 플랩 각도
        float theta = Mathf.Acos(2 * config.flapFraction - 1);
        // 이건 그냥 공식
        float flapEffectivness = 1 - (theta - Mathf.Sin(theta)) / Mathf.PI;
        //  시간당 양력 값
        float deltaLift = correctedLiftSlope * flapEffectivness * FlapEffectivnessCorrection(flapAngle) * flapAngle;
        // 양력이 0이되는 기준 값
        float zeroLiftAoaBase = config.zeroLiftAoA * Mathf.Deg2Rad;
        // 양력이 0이되는 현재 값  
        float zeroLiftAoA = zeroLiftAoaBase - deltaLift / correctedLiftSlope;

        // stall 각도 기준값
        float stallAngleHighBase = config.stallAngleHigh * Mathf.Deg2Rad;
        // stall 각도 현재 값
        float stallAngleLowBase = config.stallAngleLow * Mathf.Deg2Rad;
        // 최대 양력계수
        float clMaxHigh = correctedLiftSlope * (stallAngleHighBase - zeroLiftAoaBase) + deltaLift * LiftCoefficientMaxFraction(config.flapFraction);
        // 최소 양력계수
        float clMaxLow = correctedLiftSlope * (stallAngleLowBase - zeroLiftAoaBase) + deltaLift * LiftCoefficientMaxFraction(config.flapFraction);
        // stall 발생 최댓값
        float stallAngleHigh = zeroLiftAoA + clMaxHigh / correctedLiftSlope;
        // stall 발생 최소값
        float stallAngleLow = zeroLiftAoA + clMaxLow / correctedLiftSlope;

        // Calculating air velocity relative to the surface's coordinate system.
        // 날개 표면시스템과 연관된 공기 속도 계산부
        // Z component of the velocity is discarded. 
        // 공기 속도의 z값은 무시한다.
        
        // worldAirVelocity: 월드 공기 속도
        // airVelocity     : 월드 공기 속도의 반대 방향
        Vector3 airVelocity = transform.InverseTransformDirection(worldAirVelocity);
        //print(airVelocity);
        // 공기의 속도 벡터는 x, y 값만 받는다. (날개 오브젝트는 z방향의 윗방향이다.)
        airVelocity = new Vector3(airVelocity.x, airVelocity.y);
        // 항력벡터 정의부 방향은 플레이어 전진 방향의 반대방향
        Vector3 dragDirection = transform.TransformDirection(airVelocity.normalized);
        // 양력벡터 정의부, 방향은 플레이어 전진방향의 윗방향
        Vector3 liftDirection = Vector3.Cross(dragDirection, transform.forward);
        // 날개의 넓이
        float area = config.chord * config.span;
        // 압력값 비행기가 움직일때 날개에 작용되는 압력값
        float dynamicPressure = 0.5f * airDensity * airVelocity.sqrMagnitude;
        // 받음각
        float angleOfAttack = Mathf.Atan2(airVelocity.y, -airVelocity.x);
        // 비행역학 계수 계산기
        Vector3 aerodynamicCoefficients = CalculateCoefficients(angleOfAttack,
                                                                correctedLiftSlope,
                                                                zeroLiftAoA,
                                                                stallAngleHigh,
                                                                stallAngleLow);
        // 양력 계산부
        Vector3 lift = liftDirection * aerodynamicCoefficients.x * dynamicPressure * area;
        // 항력 계산부
        Vector3 drag = dragDirection * aerodynamicCoefficients.y * dynamicPressure * area;
        // 토크 계산부(비행기 선회를 위해거)
        Vector3 torque = -transform.forward * aerodynamicCoefficients.z * dynamicPressure * area * config.chord;
        // 비행기에게 적용시킬 최종 힘
        forceAndTorque.p += lift + drag;
        // 비행기에게 적용시킬 최종 토크
        forceAndTorque.q += Vector3.Cross(relativePosition, forceAndTorque.p);
        forceAndTorque.q += torque;

#if UNITY_EDITOR
        // For gizmos drawing.
        IsAtStall = !(angleOfAttack < stallAngleHigh && angleOfAttack > stallAngleLow);
        CurrentLift = lift;
        CurrentDrag = drag;
        CurrentTorque = torque;
#endif

        return forceAndTorque;
    }

    // ----------------------------------------------------------------------------------------------------------------------
    // 이부분은 무차원 계수를 통한 양력, 항력, 선회 모멘트를 종합적으로 산출하는 함수임 (이거 그대로 쓰면 될 듯) 
    private Vector3 CalculateCoefficients(float angleOfAttack,
                                          float correctedLiftSlope,
                                          float zeroLiftAoA,
                                          float stallAngleHigh, 
                                          float stallAngleLow)
    {
        Vector3 aerodynamicCoefficients;

        // Low angles of attack mode and stall mode curves are stitched together by a line segment. 
        float paddingAngleHigh = Mathf.Deg2Rad * Mathf.Lerp(15, 5, (Mathf.Rad2Deg * flapAngle + 50) / 100);
        float paddingAngleLow = Mathf.Deg2Rad * Mathf.Lerp(15, 5, (-Mathf.Rad2Deg * flapAngle + 50) / 100);
        float paddedStallAngleHigh = stallAngleHigh + paddingAngleHigh;
        float paddedStallAngleLow = stallAngleLow - paddingAngleLow;
        // 받음각이 stall 최대발생각보다 작고 stall 최소각보다 작을 때 
        if (angleOfAttack < stallAngleHigh && angleOfAttack > stallAngleLow)
        {
            // Low angle of attack mode.
            // 저받음각 모드
            aerodynamicCoefficients = CalculateCoefficientsAtLowAoA(angleOfAttack, correctedLiftSlope, zeroLiftAoA);
        }
        else
        {
             
            if (angleOfAttack > paddedStallAngleHigh || angleOfAttack < paddedStallAngleLow)
            {
                // Stall mode.
                aerodynamicCoefficients = CalculateCoefficientsAtStall(
                    angleOfAttack, correctedLiftSlope, zeroLiftAoA, stallAngleHigh, stallAngleLow);
            }
            else
            {
                // Linear stitching in-between stall and low angles of attack modes.
                Vector3 aerodynamicCoefficientsLow;
                Vector3 aerodynamicCoefficientsStall;
                float lerpParam;

                if (angleOfAttack > stallAngleHigh)
                {
                    aerodynamicCoefficientsLow = CalculateCoefficientsAtLowAoA(stallAngleHigh, correctedLiftSlope, zeroLiftAoA);
                    aerodynamicCoefficientsStall = CalculateCoefficientsAtStall(
                        paddedStallAngleHigh, correctedLiftSlope, zeroLiftAoA, stallAngleHigh, stallAngleLow);
                    lerpParam = (angleOfAttack - stallAngleHigh) / (paddedStallAngleHigh - stallAngleHigh);
                }
                else
                {
                    aerodynamicCoefficientsLow = CalculateCoefficientsAtLowAoA(stallAngleLow, correctedLiftSlope, zeroLiftAoA);
                    aerodynamicCoefficientsStall = CalculateCoefficientsAtStall(
                        paddedStallAngleLow, correctedLiftSlope, zeroLiftAoA, stallAngleHigh, stallAngleLow);
                    lerpParam = (angleOfAttack - stallAngleLow) / (paddedStallAngleLow - stallAngleLow);
                }
                aerodynamicCoefficients = Vector3.Lerp(aerodynamicCoefficientsLow, aerodynamicCoefficientsStall, lerpParam);
            }
        }
        return aerodynamicCoefficients;
    }

// 저 받음각 일 경우 무차원 계수 계산식
    private Vector3 CalculateCoefficientsAtLowAoA(float angleOfAttack,
                                                  float correctedLiftSlope,
                                                  float zeroLiftAoA)
    {
        float liftCoefficient = correctedLiftSlope * (angleOfAttack - zeroLiftAoA);
        float inducedAngle = liftCoefficient / (Mathf.PI * config.aspectRatio);
        float effectiveAngle = angleOfAttack - zeroLiftAoA - inducedAngle;

        float tangentialCoefficient = config.skinFriction * Mathf.Cos(effectiveAngle);
        
        float normalCoefficient = (liftCoefficient +
            Mathf.Sin(effectiveAngle) * tangentialCoefficient) / Mathf.Cos(effectiveAngle);
        float dragCoefficient = normalCoefficient * Mathf.Sin(effectiveAngle) + tangentialCoefficient * Mathf.Cos(effectiveAngle);
        float torqueCoefficient = -normalCoefficient * TorqCoefficientProportion(effectiveAngle);

        return new Vector3(liftCoefficient, dragCoefficient, torqueCoefficient);
    }

// 실속(Stall) 발생했을 때 무차원 계수 계산식
    private Vector3 CalculateCoefficientsAtStall(float angleOfAttack,
                                                 float correctedLiftSlope,
                                                 float zeroLiftAoA,
                                                 float stallAngleHigh,
                                                 float stallAngleLow)
    {
        float liftCoefficientLowAoA;
        if (angleOfAttack > stallAngleHigh)
        {
            liftCoefficientLowAoA = correctedLiftSlope * (stallAngleHigh - zeroLiftAoA);
        }
        else
        {
            liftCoefficientLowAoA = correctedLiftSlope * (stallAngleLow - zeroLiftAoA);
        }
        float inducedAngle = liftCoefficientLowAoA / (Mathf.PI * config.aspectRatio);

        float lerpParam;
        if (angleOfAttack > stallAngleHigh)
        {
            lerpParam = (Mathf.PI / 2 - Mathf.Clamp(angleOfAttack, -Mathf.PI / 2, Mathf.PI / 2))
                / (Mathf.PI / 2 - stallAngleHigh);
        }
        else
        {
            lerpParam = (-Mathf.PI / 2 - Mathf.Clamp(angleOfAttack, -Mathf.PI / 2, Mathf.PI / 2))
                / (-Mathf.PI / 2 - stallAngleLow);
        }
        inducedAngle = Mathf.Lerp(0, inducedAngle, lerpParam);
        float effectiveAngle = angleOfAttack - zeroLiftAoA - inducedAngle;

        float normalCoefficient = FrictionAt90Degrees(flapAngle) * Mathf.Sin(effectiveAngle) *
            (1 / (0.56f + 0.44f * Mathf.Abs(Mathf.Sin(effectiveAngle))) -
            0.41f * (1 - Mathf.Exp(-17 / config.aspectRatio)));
        float tangentialCoefficient = 0.5f * config.skinFriction * Mathf.Cos(effectiveAngle);

        float liftCoefficient = normalCoefficient * Mathf.Cos(effectiveAngle) - tangentialCoefficient * Mathf.Sin(effectiveAngle);
        float dragCoefficient = normalCoefficient * Mathf.Sin(effectiveAngle) + tangentialCoefficient * Mathf.Cos(effectiveAngle);
        float torqueCoefficient = -normalCoefficient * TorqCoefficientProportion(effectiveAngle);

        return new Vector3(liftCoefficient, dragCoefficient, torqueCoefficient);
    }

// 선회 모멘트 계수비율
    private float TorqCoefficientProportion(float effectiveAngle)
    {
        return 0.25f - 0.175f * (1 - 2 * Mathf.Abs(effectiveAngle) / Mathf.PI);
    }
// 플랩 각도가 수직일 때 마찰값
    private float FrictionAt90Degrees(float flapAngle)
    {
        return 1.98f - 4.26e-2f * flapAngle * flapAngle + 2.1e-1f * flapAngle;
    }
// 플랩 각도에 따른 양력 효과 계수 계산식
    private float FlapEffectivnessCorrection(float flapAngle)
    {
        return Mathf.Lerp(0.8f, 0.4f, (Mathf.Abs(flapAngle) * Mathf.Rad2Deg - 10) / 50);
    }
// 양력계수 최대값
    private float LiftCoefficientMaxFraction(float flapFraction)
    {
        return Mathf.Clamp01(1 - 0.5f * (flapFraction - 0.1f) / 0.3f);
    }

#if UNITY_EDITOR
    // For gizmos drawing.
    public AeroSurfaceConfig Config => config;
    public float GetFlapAngle() => flapAngle;
    public Vector3 CurrentLift { get; private set; }
    public Vector3 CurrentDrag { get; private set; }
    public Vector3 CurrentTorque { get; private set; }
    public bool IsAtStall { get; private set; }
#endif
}

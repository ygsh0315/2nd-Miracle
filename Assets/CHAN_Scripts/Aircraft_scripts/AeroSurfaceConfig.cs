using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aerodynamic Surface Config", menuName = "Aerodynamic Surface Config")]
public class AeroSurfaceConfig : ScriptableObject
{
    //1. liftSlope      :저 받음각에서 얼마나 많은양력을 받을 수 있는 정도 
    //2. skinFriction   :공기 마찰
    //3. zeroLiftAoA    : 양력이 0일 때 받음각
    //4. stallAngleHigh : 스톨 발생 각도
    //5. stallAngleHigh : 스톨 발생 각도 (동체가 뒤집어 졌을 때 각도 사용 용도?)
    //6. chord          : 날개의 세로 길이
    //7. flapFraction   : chord길이 대비 플랩이 차지하는 비율 
    //8. span           : 날개의 가로길이
    //9. autoAspectRatio: 날개의 가로,세로비를 자동으로 설정
    //10. aspectRatio   : 날개의 가로길이/세로길이
    public float liftSlope = 6.28f;
    public float skinFriction = 0.02f;
    public float zeroLiftAoA = 0;
    public float stallAngleHigh = 15;
    public float stallAngleLow = -15;
    public float chord = 1;
    public float flapFraction = 0;
    public float span = 1;
    public bool autoAspectRatio = true;
    public float aspectRatio = 2;

    private void OnValidate()
    {
        if (flapFraction > 0.4f)
            flapFraction = 0.4f;
        if (flapFraction < 0)
            flapFraction = 0;

        if (stallAngleHigh < 0) stallAngleHigh = 0;
        if (stallAngleLow > 0) stallAngleLow = 0;

        if (chord < 1e-3f)
            chord = 1e-3f;

        if (autoAspectRatio)
            aspectRatio = span / chord;
    }
}

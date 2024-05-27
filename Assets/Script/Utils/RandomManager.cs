using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 랜덤 게임플레이 요소를 구현할 때 사용하는 매니저
/// </summary>
public class RandomManager
{
    public static bool GetFlag(float percent) 
        => Random.value <= percent;

    public static int GetWeightedNum(int num, AnimationCurve curve) 
        => (int)(num * curve.Evaluate(Random.value));

    public static int GetElement(float[] percents)
    {
        float[] percents_copy = (float[])percents.Clone();
        float value = Random.value;

        // 확률 데이터를 범위 형태로 변경
        for (int i = 1; i < percents_copy.Length; i++)
            percents_copy[i] += percents_copy[i - 1];

        // 최대 범위에 따라 value 확장
        value *= percents_copy[percents_copy.Length - 1];

        // 범위 체크 => 당첨 요소 반환
        for (int i = 1; i < percents_copy.Length; i++)
            if (percents_copy[i - 1] <= value && value < percents_copy[i])
                return i;
        return 0;
    }

    public static int[] Shuffle(int[] list)
    {
        int[] list_copy = (int[])list.Clone();
        
        for (int i = 0; i < list_copy.Length; i++)
        {
            int randIndex = Random.Range(0, list_copy.Length);
            int temp = list_copy[randIndex];
            list_copy[randIndex] = list_copy[i];
            list_copy[i] = temp;
        }

        return list_copy;
    }

    public static List<int> ChooseSet(List<int> list, int numToSelect)
    {
        List<int> list_copy = new List<int>();

        for (int i = 0; i < numToSelect; i++)
        {
            int rand = Random.Range(0, list.Count);
            list_copy.Add(list[rand]);
            list.Remove(list[rand]);
        }

        return list_copy;
    }
}

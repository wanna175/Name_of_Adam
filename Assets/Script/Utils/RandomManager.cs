using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� �����÷��� ��Ҹ� ������ �� ����ϴ� �Ŵ���
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

        // Ȯ�� �����͸� ���� ���·� ����
        for (int i = 1; i < percents_copy.Length; i++)
            percents_copy[i] += percents_copy[i - 1];

        // �ִ� ������ ���� value Ȯ��
        value *= percents_copy[percents_copy.Length - 1];

        // ���� üũ => ��÷ ��� ��ȯ
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

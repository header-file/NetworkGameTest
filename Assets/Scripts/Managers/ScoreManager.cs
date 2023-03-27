using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public enum ScoreType
    {
        ONES = 0,
        TWOS = 1,
        THREES = 2,
        FOURS = 3,
        FIVES = 4,
        SIXES = 5,
        FOUR_OF_A_KIND = 6,
        FULL_HOUSE = 7,
        LITTLE_STRAIGHT = 8,
        BIG_STRAIGHT = 9,
        YACHT = 10,
        CHOICE = 11,
    }

    public int[] Scores;


    void Awake()
    {
        Scores = new int[12];
    }

    public void Calculate(int[] dices)
    {
        for (int i = 0; i < Scores.Length; i++)
            Scores[i] = CalculateScore(i, dices);
    }
    
    int CalculateScore(int type, int[] dices)
    {
        int score = 0;

        int[] temp = (int[])dices.Clone();
        Array.Sort(temp);
        
        switch((ScoreType)type)
        {
            case ScoreType.ONES:
                score = Upper(temp, 1);
                break;

            case ScoreType.TWOS:
                score = Upper(temp, 2);
                break;

            case ScoreType.THREES:
                score = Upper(temp, 3);
                break;

            case ScoreType.FOURS:
                score = Upper(temp, 4);
                break;

            case ScoreType.FIVES:
                score = Upper(temp, 5);
                break;

            case ScoreType.SIXES:
                score = Upper(temp, 6);
                break;

            case ScoreType.FOUR_OF_A_KIND:
                score = FOAK(temp);
                break;

            case ScoreType.FULL_HOUSE:
                score = FullHouse(temp);
                break;

            case ScoreType.LITTLE_STRAIGHT:
                score = LStraight(temp);
                break;

            case ScoreType.BIG_STRAIGHT:
                score = BStraight(temp);
                break;

            case ScoreType.YACHT:
                score = Yacht(temp);
                break;

            case ScoreType.CHOICE:
                score = Choice(temp);
                break;
        }

        return score;
    }

    int Upper(int[] dices, int num)
    {
        int score = 0;

        for(int i = 0; i < dices.Length; i++)
        {
            if (dices[i] == num)
                score += dices[i];
        }

        return score;
    }

    int FOAK(int[] dices)
    {
        int score = 0;

        if (dices[0] == dices[3] || dices[1] == dices[4])
            score = dices[1] * 4;

        return score;
    }

    int FullHouse(int[] dices)
    {
        int score = 0;

        if((dices[0] == dices[1] && dices[2] == dices[4]) ||
            (dices[0] == dices[2] && dices[3] == dices[4]))
        {
            for (int i = 0; i < dices.Length; i++)
                score += dices[i];
        }

        return score;
    }

    int LStraight(int[] dices)
    {
        int score = 30;

        for (int i = 0; i < dices.Length; i++)
            if (dices[i] != i + 1)
            {
                return 0;
            }

        return score;
    }

    int BStraight(int[] dices)
    {
        int score = 30;

        for (int i = 0; i < dices.Length; i++)
            if (dices[i] != i + 2)
            {
                return 0;
            }

        return score;
    }

    int Yacht(int[] dices)
    {
        int score = 0;

        if (dices[0] == dices[dices.Length - 1])
            score = 50;

        return score;
    }

    int Choice(int[] dices)
    {
        int score = 0;

        for (int i = 0; i < dices.Length; i++)
            score += dices[i];

        return score;
    }
}

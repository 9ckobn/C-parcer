using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Configs;
using UnityEngine;
using Random = UnityEngine.Random;

public class GetRandomWord : MonoBehaviour
{
    private GetWordPrefabs spawnWord;
    
    [SerializeField] private Config _config;
    
    private string textToParse;
    private List<char[]> wordList = new List<char[]>();

    private void Awake()
    {
        spawnWord = FindObjectOfType<GetWordPrefabs>().GetComponent<GetWordPrefabs>();
        
        textToParse = File.ReadAllText(_config.textPath).ToLower();

        string textWithoutSymbols = Regex.Replace(textToParse, "[^a-zA-Z  ]","");

        char[] separators = new char[] {' ', '\n', '\t'};

        IEnumerable<string> subStrings = textWithoutSymbols.Split(separators, StringSplitOptions.RemoveEmptyEntries).Distinct();

        foreach (string s in subStrings)
        {
            char[] word = s.ToCharArray();

            wordList.Add(word);
        }
    }

    public char[] GetWord()
    {
        try
        {
            List<char[]> charsList = wordList.Where(x => x.Length >= _config.minimalAmountLettersInWord
                                                         && x.Length <= _config.maximumAmountLettersInWord).ToList();

            char[] word = charsList[Random.Range(0, charsList.Count)];

            foreach (var item in word)
            {
                spawnWord.prefabSpawn(item);
            }
            
            Debug.Log(new string(word));
            wordList.Remove(word);
            return word;
        }
        catch (Exception e)
        {
            Debug.Log($"Text was ended with exception + {e}");
            throw;
        }
    }
}

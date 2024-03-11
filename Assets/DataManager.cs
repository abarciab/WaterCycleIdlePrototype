using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private TextAsset _objectivesTextFile;
    private List<List<string>> _objectives;

    private void Awake() {
        ParseFile();
    }

    private void ParseFile() {
        _objectives.Clear();
        var lines = _objectivesTextFile.text.Split("\n");

        for (int i = 0; i < lines.Length; i++) {
            _objectives.Add(new List<string>() {lines[i], lines[i + 1]});
            i += 1;
        }
    }

    public List<string> GetNextObjective() {
        if (_objectives.Count == 0) return null;
        
        List<string> objective = new List<string>(_objectives[0]);
        _objectives.RemoveAt(0);
        return objective;
    }  
}

using Backtrace.Unity;
using Backtrace.Unity.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
public class StartupScript : MonoBehaviour
{
    // Backtrace client instance
    private BacktraceClient _backtraceClient;
    void Start()
    {
        var serverUrl = "https://submit.backtrace.io/unity-game-trace/2046c92e0c8629067cee604de2a8bcd0c7885399315739cb937dc31b414faf2e/json";
        var gameObjectName = "Backtrace";
        var databasePath = "${Application.persistentDataPath}/sample/backtrace/path";
        var attributes = new Dictionary<string, string>() { { "my-super-cool-attribute-name", "attribute-value" } };

        // use game object to initialize Backtrace integration
        _backtraceClient = GameObject.Find(gameObjectName).GetComponent<BacktraceClient>();
        //Read from manager BacktraceClient instance
        var database = GameObject.Find(gameObjectName).GetComponent<BacktraceDatabase>();

        // or initialize Backtrace integration directly in your source code
        _backtraceClient = BacktraceClient.Initialize(
                url: serverUrl,
                databasePath: databasePath,
                gameObjectName: gameObjectName,
                attributes: attributes);
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            // throw an exception here
        }
        catch (Exception exception)
        {
            var report = new BacktraceReport(
               exception: exception,
               attributes: new Dictionary<string, string>() { { "key", "value" } },
               attachmentPaths: new List<string>() { @"file_path_1", @"file_path_2" }
           );
            _backtraceClient.Send(report);
        }
    }
}
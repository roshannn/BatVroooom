using UnityEngine;
using WAS.EventBus;

public class GameEventBusTest : MonoBehaviour {
    // Define a test query struct
    public struct TestQuery {
        public int InputValue;
    }

    void Start() {
        Debug.Log("GameEventBusTest: Starting tests...");

        // Test 1: Query with no subscribers
        int result = GameEventBus.Query<TestQuery, int>(new TestQuery { InputValue = 10 });
        if (result == 0) {
            Debug.Log("Test 1 Passed: Query with no subscribers returns default (0).");
        } else {
            Debug.LogError($"Test 1 Failed: Expected 0, got {result}");
        }

        // Test 2: Query with one subscriber
        GameEventBus.Subscribe<TestQuery, int>(OnTestQuery);
        result = GameEventBus.Query<TestQuery, int>(new TestQuery { InputValue = 10 });
        if (result == 20) {
            Debug.Log("Test 2 Passed: Query with subscriber returns correct value (20).");
        } else {
            Debug.LogError($"Test 2 Failed: Expected 20, got {result}");
        }

        // Test 3: Unsubscribe
        GameEventBus.Unsubscribe<TestQuery, int>(OnTestQuery);
        result = GameEventBus.Query<TestQuery, int>(new TestQuery { InputValue = 10 });
        if (result == 0) {
            Debug.Log("Test 3 Passed: Unsubscribe successful.");
        } else {
            Debug.LogError($"Test 3 Failed: Expected 0, got {result}");
        }

        // Test 4: Multiple subscribers (Last wins)
        GameEventBus.Subscribe<TestQuery, int>(OnTestQuery); // Returns input * 2
        GameEventBus.Subscribe<TestQuery, int>(OnTestQuery2); // Returns input * 3

        result = GameEventBus.Query<TestQuery, int>(new TestQuery { InputValue = 10 });
        if (result == 30) {
            Debug.Log("Test 4 Passed: Multiple subscribers, last one wins (30).");
        } else {
            Debug.LogError($"Test 4 Failed: Expected 30, got {result}");
        }
        
        // Cleanup
        GameEventBus.Clear();
        Debug.Log("GameEventBusTest: Tests completed.");
    }

    private int OnTestQuery(TestQuery query) {
        return query.InputValue * 2;
    }

    private int OnTestQuery2(TestQuery query) {
        return query.InputValue * 3;
    }
}

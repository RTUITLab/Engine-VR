using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class EditorTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void EditorTestsSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A Test behaves as an ordinary method
        [Test]
        public void EditorTestExample()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator EditorTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }


        [UnityTest]
        public IEnumerator EditorTestsExample()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            GameObject MovingDataBase = GameObject.Find("Двигатель(ось)");
            GameObject StaticDataBase = GameObject.Find("MovingPartsDataBase");
            //int MovingSize = MovingDataBase.GetComponent<MovingPartDataBase>().moving_Part.Count;
            //int StaticSize = StaticDataBase.GetComponent<DBFor>().moving_Part.Count;



            yield return new WaitForSeconds(5);
        }
    }
}

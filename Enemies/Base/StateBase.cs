using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface StateBase
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
}
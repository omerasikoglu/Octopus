using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollider 
{
    void SetBoolIsGrounded(bool isGrounded);
    void SetBoolOnRightWall(bool onRightWall);
    void SetBoolOnLeftWall(bool onLeftWall);
    void SetBoolOnRightLedgeCheck(bool rightLedgeCheck);
    void SetBoolOnLeftLedgeCheck(bool leftLedgeCheck);
}

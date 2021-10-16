using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformColliders : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float extraHeightText = .3f;

    private ICollider colliderYo; //YO!

    private BoxCollider2D boxCollider2d;
    private float wallCheckDistance = .3f;

    private void Awake()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        colliderYo = GetComponent<ICollider>();
    }
    private void FixedUpdate()
    {
        colliderYo.SetBoolIsGrounded(IsGrounded());
        colliderYo.SetBoolOnRightWall(OnRightWall());
        colliderYo.SetBoolOnLeftWall(OnLeftWall());
        colliderYo.SetBoolOnRightLedgeCheck(RightLedgeCheck());
        colliderYo.SetBoolOnLeftLedgeCheck(LeftLedgeCheck());
    }
    private bool RightLedgeCheck()
    {
        RaycastHit2D raycastHitDown = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 2, 0),
            new Vector3(boxCollider2d.bounds.extents.x, .1f/*boxCollider2d.bounds.extents.y/4*2*/, 0),
            0f, Vector2.right, wallCheckDistance, platformLayer);
        RaycastHit2D raycastHitUp = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 3, 0), new Vector3(boxCollider2d.bounds.extents.x, .1f/*boxCollider2d.bounds.extents.y / 4 * 2*/, 0), 0f, Vector2.right, wallCheckDistance, platformLayer);
        if (raycastHitDown.collider != null && raycastHitUp.collider == null)
        {
            return true;
        }
        else return false;
    }
    private bool LeftLedgeCheck()
    {
        RaycastHit2D raycastHitDown = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 2, 0), new Vector3(boxCollider2d.bounds.extents.x, .1f/*boxCollider2d.bounds.extents.y / 4 * 2*/, 0), 0f, Vector2.left, wallCheckDistance, platformLayer);
        RaycastHit2D raycastHitUp = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 3, 0), new Vector3(boxCollider2d.bounds.extents.x, .1f/*boxCollider2d.bounds.extents.y / 4 * 2*/, 0), 0f, Vector2.left, wallCheckDistance, platformLayer);
        if (raycastHitDown.collider != null && raycastHitUp.collider == null)
        {
            return true;
        }
        else return false;
    }
    private bool OnRightWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y / 4, 0),
            new Vector3(boxCollider2d.bounds.extents.x, .1f, 0),
            0f, Vector2.right, wallCheckDistance, platformLayer);
        Color rayColor;
        if (raycastHit.collider != null) rayColor = Color.yellow;
        else rayColor = Color.red;
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y),
            Vector2.right * (boxCollider2d.bounds.extents.x + wallCheckDistance), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, (boxCollider2d.bounds.extents.y) / 2), Vector2.right * (boxCollider2d.bounds.extents.x + wallCheckDistance), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3((boxCollider2d.bounds.extents.x) + wallCheckDistance, boxCollider2d.bounds.extents.y), Vector2.down * ((3 * boxCollider2d.bounds.extents.y) / 2), rayColor);

        return raycastHit.collider != null;
    }
    private bool OnLeftWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y / 4, 0),
            new Vector3(boxCollider2d.bounds.extents.x, .1f, 0), 
            0f, Vector2.left, wallCheckDistance, platformLayer);
        Color rayColor;
        if (raycastHit.collider != null) rayColor = Color.cyan;
        else rayColor = Color.red;
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y), Vector2.left * (boxCollider2d.bounds.extents.x + wallCheckDistance), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, (boxCollider2d.bounds.extents.y) / 2), Vector2.left * (boxCollider2d.bounds.extents.x + wallCheckDistance), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3((-boxCollider2d.bounds.extents.x) - wallCheckDistance, boxCollider2d.bounds.extents.y), Vector2.down * ((3 * boxCollider2d.bounds.extents.y) / 2), rayColor);

        return raycastHit.collider != null;
    }
    private bool IsGrounded()
    {
        //float extraHeightText = .3f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size - new Vector3(.2f, 0f, 0f), 0f, Vector2.down, extraHeightText, platformLayer);
        Color rayColor;
        if (raycastHit.collider != null) rayColor = Color.green;
        else rayColor = Color.red;

        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x * 2f), rayColor);
        //Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }

}
//private void OnDrawGizmos()
//{
//    Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
//}

//ESKİSİ
//private bool RightLedgeCheck()
//{
//    RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center - new Vector3(0, (boxCollider2d.bounds.extents.y) / 4, 0), new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y / 4 * 3, 0), 0f, Vector2.right, wallCheckDistance, platformLayer);
//    RaycastHit2D raycastHit_2 = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 3, 0), new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y / 4 * 2, 0), 0f, Vector2.right, wallCheckDistance, platformLayer);
//    if (raycastHit.collider != null && raycastHit_2.collider == null)
//    {
//        return true;
//    }
//    else return false;
//}
//private bool LeftLedgeCheck()
//{
//    RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center - new Vector3(0, (boxCollider2d.bounds.extents.y) / 4, 0), new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y / 4 * 3, 0), 0f, Vector2.left, wallCheckDistance, platformLayer);
//    RaycastHit2D raycastHit_2 = Physics2D.BoxCast(boxCollider2d.bounds.center + new Vector3(0, (boxCollider2d.bounds.extents.y) / 4 * 3, 0), new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y / 4 * 2, 0), 0f, Vector2.left, wallCheckDistance, platformLayer);
//    if (raycastHit.collider != null && raycastHit_2.collider == null)
//    {
//        return true;
//    }
//    else return false;
//}
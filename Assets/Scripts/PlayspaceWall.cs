using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayspaceWall : MonoBehaviour {
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "PlayerBullet") {
            Destroy(other.gameObject);
        }
    }
}

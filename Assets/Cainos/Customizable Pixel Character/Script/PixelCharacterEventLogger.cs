using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{

    public class PixelCharacterEventLogger : MonoBehaviour
    {
        private PixelCharacterController controller;

        private void Awake()
        {
            controller = GetComponent<PixelCharacterController>();
            controller.onFootstep.AddListener(()=>{ Debug.Log("Footstep");});
            controller.onJump.AddListener(() => { Debug.Log("Jump"); });
            controller.onLand.AddListener(() => { Debug.Log("Land"); });
        }
    }
}

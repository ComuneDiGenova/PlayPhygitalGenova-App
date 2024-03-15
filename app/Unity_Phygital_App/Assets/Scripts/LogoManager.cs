using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LogoManager : MonoBehaviour
{
    const string imageURL = "/sites/default/files/Icona_G.png";
    [SerializeField] Image logoImage;

    private void Start() {
        //image url
        WSO2.GETImage(AuthorizationAPI.baseURL+imageURL,(texture) => {
            var sprite = Sprite.Create(texture,new Rect(0,0,texture.width,texture.height),Vector2.one/2,100);
            if(texture != null)
                logoImage.sprite = sprite;
        });
    }
}

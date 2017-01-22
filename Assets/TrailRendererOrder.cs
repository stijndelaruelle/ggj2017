using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererOrder : MonoBehaviour {

    [SerializeField] string Layer;
    [SerializeField] int Order;

    void Start() {
        GetComponent<TrailRenderer>().sortingLayerName = Layer;
        GetComponent<TrailRenderer>().sortingOrder = Order;
    }
}

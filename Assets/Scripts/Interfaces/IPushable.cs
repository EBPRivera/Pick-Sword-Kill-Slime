using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable {
    public void Push(Vector3 pusherPosition);

    public void Push(Vector3 pusherPosition, float distance);
}

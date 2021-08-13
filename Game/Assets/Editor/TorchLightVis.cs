using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (TorchLightController))]
public class TorchLightVis : Editor {

	void OnSceneGUI() {
		TorchLightController fow = (TorchLightController)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);

        //Draws cone of view
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);
		
		Handles.color = Color.red;
		foreach (Transform target in fow.visibleTargets) {
			Handles.DrawLine (fow.transform.position, target.position);
		}
	}

}
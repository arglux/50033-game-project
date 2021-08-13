using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "EnemyAI/State")]
public class State : ScriptableObject 
{

    public Action[] actions;
    public Transition[] transitions;

    // debugger
    public Color gizmoColor = Color.grey;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller)
    {
        for (int i = 0; i < actions.Length; i++) 
        {
            actions[i].Act(controller);
        }
    }
    private void CheckTransitions(StateController controller)
    {
        controller.transitionStateChanged = false; //reset 
        for (int i = 0; i < transitions.Length; i++) 
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);

            //check if the previous transition has caused a change. 
            // If yes, stop. Let Update() in StateController run again in the next state. 
            if (controller.transitionStateChanged) break;

            if (decisionSucceeded) 
            {
                controller.TransitionToState(transitions[i].trueState);
            } else 
            {
                controller.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
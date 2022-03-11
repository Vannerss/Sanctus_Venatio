using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatHandler : MonoBehaviour
{

    [SerializeField]
    private InputActionReference attackControl;

    private void OnEnable()
    {
        attackControl.action.Enable();
    }

    private void OnDisable()
    {
        attackControl.action.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}

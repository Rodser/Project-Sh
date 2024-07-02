//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.2
//     from Assets/InputSettings/InputControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

public partial class @InputControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControl"",
    ""maps"": [
        {
            ""name"": ""Action map"",
            ""id"": ""878b6b31-f039-4cac-80cb-84add81666a6"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""3505bb4f-e276-4883-ba3b-0414b4725a90"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""1480fff1-a106-4816-bf73-6d766058c1a4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""37cea420-1a94-4477-b0ef-0aa00669041d"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""961176a5-1cd0-4543-a4cd-5f1e60030bd9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ca2a4730-cd92-45dd-b5fb-e55c9ab7f81c"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b622ee5-f270-4d66-9c22-ff9f3a8c3167"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Action map
        m_Actionmap = asset.FindActionMap("Action map", throwIfNotFound: true);
        m_Actionmap_Click = m_Actionmap.FindAction("Click", throwIfNotFound: true);
        m_Actionmap_Position = m_Actionmap.FindAction("Position", throwIfNotFound: true);
    }

    ~@InputControl()
    {
        Debug.Assert(!m_Actionmap.enabled, "This will cause a leak and performance issues, InputControl.Actionmap.Disable() has not been called.");
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Action map
    private readonly InputActionMap m_Actionmap;
    private List<IActionmapActions> m_ActionmapActionsCallbackInterfaces = new List<IActionmapActions>();
    private readonly InputAction m_Actionmap_Click;
    private readonly InputAction m_Actionmap_Position;
    public struct ActionmapActions
    {
        private @InputControl m_Wrapper;
        public ActionmapActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_Actionmap_Click;
        public InputAction @Position => m_Wrapper.m_Actionmap_Position;
        public InputActionMap Get() { return m_Wrapper.m_Actionmap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionmapActions set) { return set.Get(); }
        public void AddCallbacks(IActionmapActions instance)
        {
            if (instance == null || m_Wrapper.m_ActionmapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ActionmapActionsCallbackInterfaces.Add(instance);
            @Click.started += instance.OnClick;
            @Click.performed += instance.OnClick;
            @Click.canceled += instance.OnClick;
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
        }

        private void UnregisterCallbacks(IActionmapActions instance)
        {
            @Click.started -= instance.OnClick;
            @Click.performed -= instance.OnClick;
            @Click.canceled -= instance.OnClick;
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
        }

        public void RemoveCallbacks(IActionmapActions instance)
        {
            if (m_Wrapper.m_ActionmapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IActionmapActions instance)
        {
            foreach (var item in m_Wrapper.m_ActionmapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ActionmapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ActionmapActions @Actionmap => new ActionmapActions(this);
    public interface IActionmapActions
    {
        void OnClick(InputAction.CallbackContext context);
        void OnPosition(InputAction.CallbackContext context);
    }
}

// Copyright (c) 2016 Augie R. Maddox, Guavaman Enterprises. All rights reserved.
// This file must be placed inside the Assets/Plugins folder

using System;
using UnityEngine;

#if !REWIRED_DISABLE_UNITY_INPUT_OVERRIDE

/// <summary>
/// Overrides the UnityEngine.Input class and re-routes GetButton and GetAxis calls to Rewired Player 0.
/// This only works if scripts call Input without explicitly specifying UnityEngine.Input.
/// </summary>
public static class Input {

    #region Rewired Members
       
    private static bool rewiredError = false;
#if UNITY_EDITOR
    private static bool logged = false;
#endif

    private static Rewired.Player player {
        get {
            if(!Rewired.UnityInputOverride.enabled) return null;
            if(!Rewired.ReInput.isReady) {
                // Log the error once per session
                if(!rewiredError) {
                    rewiredError = true;
                    Debug.LogWarning("Rewired: Error overriding Unity input. Rewired is not initialized! Do you have a Rewired Input Manager in the scene? Input calls will be handled by UnityEngine.Input.");
                }
                return null;
            }
            if(Rewired.ReInput.players.playerCount <= 0) {
                // Log the error once per session
                if(!rewiredError) {
                    rewiredError = true;
                    Debug.LogWarning("Rewired: Error overriding Unity input. There are no Rewired Players defined! You must have at least one Rewired Player to override Unity input. Input calls will be handled by UnityEngine.Input.");
                }
                return null;
            }

#if UNITY_EDITOR
            if(!logged) {
                Debug.Log("Rewired: Currently overriding Unity input. To disable the Unity input override permanently, delete Assets/Plugins/Rewired/UnityInputOverride.cs.");
                logged = true;
            }
#endif
            return Rewired.ReInput.players.GetPlayer(Rewired.UnityInputOverride.playerId);
        }
    }

    #endregion

    /// <summary>
    /// Last measured linear acceleration of a device in three-dimensional space.(Read Only)
    /// </summary>
    /// <returns></returns>
    public static Vector3 acceleration { get { return UnityEngine.Input.acceleration; } }

    /// <summary>
    /// Number of acceleration measurements which occurred during last frame.
    /// </summary>
    /// <returns></returns>
    public static int accelerationEventCount { get { return UnityEngine.Input.accelerationEventCount; } }
    //
    /// <summary>
    /// Returns list of acceleration measurements which occurred during the last frame. (Read Only) (Allocates temporary variables).
    /// </summary>
    /// <returns></returns>
    public static AccelerationEvent[] accelerationEvents { get { return UnityEngine.Input.accelerationEvents; } }

    /// <summary>
    /// Is any key or mouse button currently held down? (Read Only)
    /// </summary>
    /// <returns></returns>
    public static bool anyKey { get { return UnityEngine.Input.anyKey; } }

    /// <summary>
    /// Returns true the first frame the user hits any key or mouse button. (Read Only)
    /// </summary>
    /// <returns></returns>
    public static bool anyKeyDown { get { return UnityEngine.Input.anyKeyDown; } }

    /// <summary>
    /// Should Back button quit the application? Only usable on Android, Windows Phone or Windows Tablets.
    /// </summary>
    /// <returns></returns>
    public static bool backButtonLeavesApp {
        get {
#if UNITY_5_3_OR_NEWER
            return UnityEngine.Input.backButtonLeavesApp;
#else
            throw new NotSupportedException();
#endif
        }
        set {
#if UNITY_5_3_OR_NEWER
            UnityEngine.Input.backButtonLeavesApp = value;
#else
            throw new NotSupportedException();
#endif
        }
    }

    /// <summary>
    /// Property for accessing compass (handheld devices only). (Read Only)
    /// </summary>
    /// <returns></returns>
    public static Compass compass { get { return UnityEngine.Input.compass; } }

    /// <summary>
    /// This property controls if input sensors should be compensated for screen orientation.
    /// </summary>
    /// <returns></returns>
    public static bool compensateSensors {
        get { return UnityEngine.Input.compensateSensors; }
        set { UnityEngine.Input.compensateSensors = value; }
    }

    /// <summary>
    /// The current text input position used by IMEs to open windows.
    /// </summary>
    /// <returns></returns>
    public static Vector2 compositionCursorPos {
        get { return UnityEngine.Input.compositionCursorPos; }
        set { UnityEngine.Input.compositionCursorPos = value; }
    }

    /// <summary>
    /// The current IME composition string being typed by the user.
    /// </summary>
    /// <returns></returns>
    public static string compositionString { get { return UnityEngine.Input.compositionString; } }

    /// <summary>
    /// Device physical orientation as reported by OS. (Read Only)
    /// </summary>
    /// <returns></returns>
    public static DeviceOrientation deviceOrientation { get { return UnityEngine.Input.deviceOrientation; } }

    /// <summary>
    /// Property indicating whether keypresses are eaten by a textinput if it has focus (default true).
    /// </summary>
    /// <returns></returns>
    [Obsolete("eatKeyPressOnTextFieldFocus property is deprecated, and only provided to support legacy behavior.")]
    public static bool eatKeyPressOnTextFieldFocus {
        get { return UnityEngine.Input.eatKeyPressOnTextFieldFocus; }
        set { UnityEngine.Input.eatKeyPressOnTextFieldFocus = value; }
    }

    /// <summary>
    /// Returns default gyroscope.
    /// </summary>
    /// <returns></returns>
    public static Gyroscope gyro { get { return UnityEngine.Input.gyro; } }

    /// <summary>
    /// Controls enabling and disabling of IME input composition.
    /// </summary>
    /// <returns></returns>
    public static IMECompositionMode imeCompositionMode {
        get { return UnityEngine.Input.imeCompositionMode; }
        set { UnityEngine.Input.imeCompositionMode = value; }
    }

    /// <summary>
    /// Does the user have an IME keyboard input source selected?
    /// </summary>
    /// <returns></returns>
    public static bool imeIsSelected { get { return UnityEngine.Input.imeIsSelected; } }
    //
    /// <summary>
    /// Returns the keyboard input entered this frame. (Read Only)
    /// </summary>
    /// <returns></returns>
    public static string inputString { get { return UnityEngine.Input.inputString; } }

    [Obsolete("isGyroAvailable property is deprecated. Please use SystemInfo.supportsGyroscope instead.")]
    public static bool isGyroAvailable { get { return UnityEngine.Input.isGyroAvailable; } }

#if !REWIRED_UNITY_INPUT_OVERRIDE_DISABLE_LOCATION_SERVICE
    /// <summary>
    /// Property for accessing device location (handheld devices only). (Read Only)
    /// </summary>
    /// <returns></returns>
    public static LocationService location { get { return UnityEngine.Input.location; } }
#endif

    /// <summary>
    /// The current mouse position in pixel coordinates. (Read Only)
    /// </summary>
    /// <returns></returns>
    public static Vector3 mousePosition { get { return UnityEngine.Input.mousePosition; } }
    public static bool mousePresent { get { return UnityEngine.Input.mousePresent; } }

    /// <summary>
    /// The current mouse scroll delta. (Read Only)
    /// </summary>
    /// <returns></returns>
    public static Vector2 mouseScrollDelta {
        get {
#if UNITY_5 || UNITY_5_3_OR_NEWER || (UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
            return UnityEngine.Input.mouseScrollDelta;
#else
            throw new NotSupportedException();
#endif
        }
    }

    /// <summary>
    /// Property indicating whether the system handles multiple touches.
    /// </summary>
    /// <returns></returns>
    public static bool multiTouchEnabled {
        get { return UnityEngine.Input.multiTouchEnabled; }
        set { UnityEngine.Input.multiTouchEnabled = value; }
    }

    /// <summary>
    /// Enables/Disables mouse simulation with touches. By default this option is enabled.
    /// </summary>
    /// <returns></returns>
    public static bool simulateMouseWithTouches {
        get { return UnityEngine.Input.simulateMouseWithTouches; }
        set { UnityEngine.Input.simulateMouseWithTouches = value; }
    }

    /// <summary>
    /// Returns true when Stylus Touch is supported by a device or platform.
    /// </summary>
    /// <returns></returns>
    public static bool stylusTouchSupported {
        get {
#if UNITY_5_3_OR_NEWER
            return UnityEngine.Input.stylusTouchSupported;
#else
            throw new NotSupportedException();
#endif
        }
    }

    /// <summary>
    /// Number of touches. Guaranteed not to change throughout the frame. (Read Only)
    /// </summary>
    /// <returns></returns>
    public static int touchCount { get { return UnityEngine.Input.touchCount; } }

    /// <summary>
    /// Returns list of objects representing status of all touches during last frame. (Read Only) (Allocates temporary variables).
    /// </summary>
    /// <returns></returns>
    public static Touch[] touches { get { return UnityEngine.Input.touches; } }

    /// <summary>
    /// Bool value which let's users check if touch pressure is supported.
    /// </summary>
    /// <returns></returns>
    public static bool touchPressureSupported {
        get {
#if UNITY_5_3_OR_NEWER
            return UnityEngine.Input.touchPressureSupported;
#else
            throw new NotSupportedException();
#endif
        }
    }

    /// <summary>
    /// Returns whether the device on which application is currently running supports touch input.
    /// </summary>
    /// <returns></returns>
    public static bool touchSupported {
        get {
#if UNITY_5 || UNITY_5_3_OR_NEWER || (UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9)
            return UnityEngine.Input.touchSupported;
#else
            throw new NotSupportedException();
#endif
        }
    }

    /// <summary>
    /// Returns specific acceleration measurement which occurred during last frame. (Does not allocate temporary variables).
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static AccelerationEvent GetAccelerationEvent(int index) {
        return UnityEngine.Input.GetAccelerationEvent(index);
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by axisName.
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    public static float GetAxis(string axisName) {
        if(player == null) return UnityEngine.Input.GetAxis(axisName);
        return player.GetAxis(axisName);
    }

    /// <summary>
    /// Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.
    /// </summary>
    /// <param name="axisName"></param>
    /// <returns></returns>
    public static float GetAxisRaw(string axisName) {
        if(player == null) return UnityEngine.Input.GetAxisRaw(axisName);
        return player.GetAxisRaw(axisName);
    }

    /// <summary>
    /// Returns true while the virtual button identified by buttonName is held down.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButton(string buttonName) {
        if(player == null) return UnityEngine.Input.GetButton(buttonName);
        return player.GetButton(buttonName);
    }

    /// <summary>
    /// Returns true during the frame the user pressed down the virtual button identified by buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonDown(string buttonName) {
        if(player == null) return UnityEngine.Input.GetButtonDown(buttonName);
        return player.GetButtonDown(buttonName);
    }

    /// <summary>
    /// Returns true the first frame the user releases the virtual button identified by buttonName.
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    public static bool GetButtonUp(string buttonName) {
        if(player == null) return UnityEngine.Input.GetButtonUp(buttonName);
        return player.GetButtonUp(buttonName);
    }

    /// <summary>
    /// Returns an array of strings describing the connected joysticks.
    /// </summary>
    /// <returns></returns>
    public static string[] GetJoystickNames() {
        if(!Rewired.UnityInputOverride.enabled || !Rewired.ReInput.isReady) return UnityEngine.Input.GetJoystickNames();
        int count = Rewired.ReInput.controllers.joystickCount;
        string[] names = new string[count];
        for(int i = 0; i < count; i++) {
            names[i] = Rewired.ReInput.controllers.Joysticks[i].name;
        }
        return names;
    }

    /// <summary>
    /// Returns true while the user holds down the key identified by the key KeyCode enum parameter.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKey(KeyCode key) {
        return UnityEngine.Input.GetKey(key);
    }

    /// <summary>
    /// Returns true while the user holds down the key identified by name. Think auto fire.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool GetKey(string name) {
        return UnityEngine.Input.GetKey(name);
    }

    /// <summary>
    /// Returns true during the frame the user starts pressing down the key identified by the key KeyCode enum parameter.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyDown(KeyCode key) {
        return UnityEngine.Input.GetKeyDown(key);
    }

    /// <summary>
    /// Returns true during the frame the user starts pressing down the key identified by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool GetKeyDown(string name) {
        return UnityEngine.Input.GetKeyDown(name);
    }

    /// <summary>
    /// Returns true during the frame the user releases the key identified by the key KeyCode enum parameter.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool GetKeyUp(KeyCode key) {
        return UnityEngine.Input.GetKeyUp(key);
    }

    /// <summary>
    /// Returns true during the frame the user releases the key identified by name.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static bool GetKeyUp(string name) {
        return UnityEngine.Input.GetKeyUp(name);
    }

    /// <summary>
    /// Returns whether the given mouse button is held down.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool GetMouseButton(int button) {
        if(!Rewired.UnityInputOverride.enabled || !Rewired.ReInput.isReady) return UnityEngine.Input.GetMouseButton(button);
        return Rewired.ReInput.controllers.Mouse.GetButton(button);
    }

    /// <summary>
    /// Returns true during the frame the user pressed the given mouse button.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool GetMouseButtonDown(int button) {
        if(!Rewired.UnityInputOverride.enabled || !Rewired.ReInput.isReady) return UnityEngine.Input.GetMouseButtonDown(button);
        return Rewired.ReInput.controllers.Mouse.GetButtonDown(button);
    }

    /// <summary>
    //  Returns true during the frame the user releases the given mouse button.
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public static bool GetMouseButtonUp(int button) {
        if(!Rewired.UnityInputOverride.enabled || !Rewired.ReInput.isReady) return UnityEngine.Input.GetMouseButtonUp(button);
        return Rewired.ReInput.controllers.Mouse.GetButtonUp(button);
    }

    [Obsolete("Use ps3 move API instead", true)]
    public static Vector3 GetPosition(int deviceID) {
        throw new NotSupportedException();
    }

    [Obsolete("Use ps3 move API instead", true)]
    public static Quaternion GetRotation(int deviceID) {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Returns object representing status of a specific touch. (Does not allocate temporary variables).
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static Touch GetTouch(int index) {
        return UnityEngine.Input.GetTouch(index);
    }

    /// <summary>
    /// Determine whether a particular joystick model has been preconfigured by Unity. (Linux-only).
    /// </summary>
    /// <param name="joystickName">The name of the joystick to check (returned by Input.GetJoystickNames).</param>
    /// <returns>True if the joystick layout has been preconfigured; false otherwise.</returns>
    public static bool IsJoystickPreconfigured(string joystickName) {
#if UNITY_STANDALONE_LINUX && (UNITY_5 || UNITY_5_3_OR_NEWER)
        return UnityEngine.Input.IsJoystickPreconfigured(joystickName);
#else
        return false;
#endif
    }

    /// <summary>
    /// Resets all input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame.
    /// </summary>
    public static void ResetInputAxes() {
        UnityEngine.Input.ResetInputAxes();
    }
}

#endif

namespace Rewired {
    public static class UnityInputOverride {
    
#if !REWIRED_DISABLE_UNITY_INPUT_OVERRIDE
        private static bool _enabled = true;
#else
        private static bool _enabled = false;
#endif
        private static int _playerId = 0;
        
        /// <summary>
        /// Enables or disables Unity Input Override.
        /// If enabled, input will be retrieved from Rewired.
        /// If disabled, input will be retrieved from UnityEngine.Input.
        /// </summary>
        public static bool enabled {
            get { return _enabled; }
#if !REWIRED_DISABLE_UNITY_INPUT_OVERRIDE
            set { _enabled = value; }
#else
            set { Debug.LogWarning("Rewired: UnityInputOverride has been permanently disabled in the script."); }
#endif
        }
        
        /// <summary>
        /// Gets or sets the player id for the Rewired Player from which to get all input.
        /// </summary>
        public static int playerId {
            get { return _playerId; }
#if !REWIRED_DISABLE_UNITY_INPUT_OVERRIDE
            set {
                if(!Rewired.ReInput.isReady) return;
                if(Rewired.ReInput.players.GetPlayer(value) == null) return;
                _playerId = value;
            }
#else
            set { Debug.LogWarning("Rewired: UnityInputOverride has been permanently disabled in the script."); }
#endif
        }
    }
}
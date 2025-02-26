using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioMovement : MonoBehaviour
{
    // Original SMW values (in units per 1/60th second)
    public float minWalkingSpeed = HexToFloat(0x00130);
    public float maxWalkingSpeed = HexToFloat(0x01900);
    public float maxWalkingSpeedLevelEntry = HexToFloat(0x00D00);
    public float walkingAcceleration = HexToFloat(0x00098);

    public float maxRunningSpeed = HexToFloat(0x02900);
    public float runningAcceleration = HexToFloat(0x000E4);

    public float releaseDeceleration = HexToFloat(0x000D0);

    public float skidDeceleration = HexToFloat(0x001A0);
    public float skidTurnaroundSpeed = HexToFloat(0x00900);

    public float xAxisInput = 0;
    public bool isRunning = false;

    private int runningCountdown = 0;

    private Rigidbody2D _rb;
    private const float SmwFramerate = 60f; // SMW runs at 60 FPS

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // Get current velocity (already in m/s in Unity)
        var velocity = _rb.linearVelocity;

        // running countdown
        if (!isRunning && runningCountdown > 0)
        {
            runningCountdown--;
        }

        // Convert current Unity velocity to SMW units per frame
        var currentSpeedInSmwUnits = velocity.x / SmwFramerate;

        // Calculate new speed in SMW units per frame
        var newSpeedInSmwUnits = CalculateNewSpeed(currentSpeedInSmwUnits);

        // if speed is below min walking speed, set it to 0
        if (Mathf.Abs(newSpeedInSmwUnits) < minWalkingSpeed)
        {
            newSpeedInSmwUnits = 0;
        }

        // Convert back to Unity's m/s
        velocity.x = newSpeedInSmwUnits * SmwFramerate;

        // Apply the calculated velocity
        _rb.linearVelocity = velocity;
    }

    private float CalculateNewSpeed(float currentSpeed)
    {
        // horizontal movement
        // if no input is given, decelerate the player
        if (xAxisInput == 0)
        {
            return currentSpeed switch
            {
                > 0 => Mathf.Max(currentSpeed - releaseDeceleration, 0),
                < 0 => Mathf.Min(currentSpeed + releaseDeceleration, 0),
                _ => currentSpeed
            };
        }
        var newSpeed = currentSpeed;

        // if input points against the current direction
        if (Math.Sign(xAxisInput) != Math.Sign(currentSpeed))
        {
            // if the current speed is below skid turnaround speed turn it around and set the speed to min walking speed
            if (Mathf.Abs(currentSpeed) < skidTurnaroundSpeed)
            {
                newSpeed = Mathf.Sign(xAxisInput) * minWalkingSpeed;
            }
            else
            {
                // if the current speed is above skid turnaround speed, decelerate the player
                newSpeed = currentSpeed switch
                {
                    > 0 => Mathf.Max(currentSpeed - skidDeceleration, 0),
                    < 0 => Mathf.Min(currentSpeed + skidDeceleration, 0),
                    _ => currentSpeed
                };
            }
        }
        else
        {
            // accelerate the player
            newSpeed += xAxisInput * (isRunning || runningCountdown > 0 ? runningAcceleration : walkingAcceleration);
        }

        // Clamp the speed to the max walking speed or max running speed (ensure it respects direction)
        var maxSpeed = isRunning || runningCountdown > 0 ? maxRunningSpeed : maxWalkingSpeed;
        newSpeed = Mathf.Clamp(newSpeed, -maxSpeed, maxSpeed);

        return newSpeed;
    }

    // callback for the horizontal input with unity's input system
    public void OnXInput(InputAction.CallbackContext context)
    {
        xAxisInput = context.ReadValue<Vector2>().x;
    }

    // callback for the run button with unity's input system
    public void OnRun(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValue<float>() > 0;
        if (isRunning)
        {
            runningCountdown = 10;
        }
    }

    private static float HexToFloat(int hex)
    {
        // Extract each hexadecimal digit using bit operations
        var a = (hex >> 16) & 0xF;
        var b = (hex >> 12) & 0xF;
        var c = (hex >> 8) & 0xF;
        var d = (hex >> 4) & 0xF;
        var e = hex & 0xF;

        // Precompute division constants for better performance
        const float div16 = 1.0f / 16.0f;
        const float div256 = 1.0f / 256.0f;
        const float div4096 = 1.0f / 4096.0f;
        const float div65536 = 1.0f / 65536.0f;

        // Calculate result using the formula with multiplication instead of division
        return a + b * div16 + c * div256 + d * div4096 + e * div65536;
    }
}

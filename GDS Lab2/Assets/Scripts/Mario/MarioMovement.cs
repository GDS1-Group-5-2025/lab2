using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MarioMovement : MonoBehaviour
{
    // Original SMW values (in units per 1/60th second)
    // Walking
    public float minWalkingSpeed = HexToFloat(0x00130);
    public float maxWalkingSpeed = HexToFloat(0x01900);
    public float maxWalkingSpeedLevelEntry = HexToFloat(0x00D00);
    public float walkingAcceleration = HexToFloat(0x00098);

    // Running
    public float maxRunningSpeed = HexToFloat(0x02900);
    public float runningAcceleration = HexToFloat(0x000E4);

    // Skidding
    public float releaseDeceleration = HexToFloat(0x000D0);

    public float skidDeceleration = HexToFloat(0x001A0);
    public float skidTurnaroundSpeed = HexToFloat(0x00900);

    // Momentum
    public float momentumDecelerationSmall = HexToFloat(0x00098); // speed < 0x01900 and started jump with speed < 0x01D00
    public float momentumDecelerationMedium = HexToFloat(0x000D0); // speed < 0x01900 and started jump with speed >= 0x01D00
    public float momentumDecelerationLarge = HexToFloat(0x000E4); // speed >= 0x01900

    public float momentumAccelerationSmall = HexToFloat(0x00098); // speed < 0x01900
    public float momentumAccelerationLarge = HexToFloat(0x000E4); // speed >= 0x01900

    public float maxAirSpeedSmall = HexToFloat(0x01900); // startspeed < 0x01900
    public float maxAirSpeedLarge = HexToFloat(0x02900); // startspeed >= 0x01900

    // Jumping - initial upward speed
    public float jumpSpeedSmall = HexToFloat(0x04000); // speed < 0x01000
    public float jumpSpeedMedium = HexToFloat(0x04000); // speed >= 0x01000 and < 0x02500
    public float jumpSpeedLarge = HexToFloat(0x05000); // speed >= 0x02500

    // Jumping - normal gravity
    public float normalGravitySmall = HexToFloat(0x00700); // speed < 0x01000
    public float normalGravityMedium = HexToFloat(0x00600); // speed >= 0x01000 and < 0x02500
    public float normalGravityLarge = HexToFloat(0x00900); // speed >= 0x02500

    // Jumping - dampened gravity
    public float dampenedGravitySmall = HexToFloat(0x00200); // speed < 0x01000
    public float dampenedGravityMedium = HexToFloat(0x001E0); // speed >= 0x01000 and < 0x02500
    public float dampenedGravityLarge = HexToFloat(0x00280); // speed >= 0x02500

    // Input
    public float xAxisInput;
    public bool isRunning;
    public bool isJumping;

    // State
    private int _runningCountdown;
    public float initialXSpeedWhenJumping;
    public float lastGravity = HexToFloat(0x00280); // default gravity
    public float lastDampenedGravity = HexToFloat(0x00280); // default dampened gravity

    private Rigidbody2D _rb;
    private const float SmwFramerate = 60f; // SMW runs at 60 FPS
    private float _accumulator; // Track leftover time
    private const float FixedTimeStep = 1f / 60f; // 60Hz physics update

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Accumulate time
        _accumulator += Time.deltaTime;

        // Run physics updates at fixed timestep
        while (_accumulator >= FixedTimeStep)
        {
            UpdatePhysics();
            _accumulator -= FixedTimeStep;
        }
    }

    // Physics update at fixed timestep
    private void UpdatePhysics()
    {
        // Get current velocity (already in m/s in Unity)
        var velocity = _rb.linearVelocity;

        // running countdown
        if (!isRunning && _runningCountdown > 0)
        {
            _runningCountdown--;
        }

        // Convert current Unity velocity to SMW units per frame
        var currentXSpeedInSmwUnits = velocity.x / SmwFramerate;
        var currentYSpeedInSmwUnits = velocity.y / SmwFramerate;

        float newXSpeedInSmwUnits;

        // We're on the ground if the vertical speed is close to 0
        if (Mathf.Abs(currentYSpeedInSmwUnits) < 0.1f)
        {
            // Calculate new speed in SMW units per frame
            newXSpeedInSmwUnits = CalculateNewXGroundSpeed(currentXSpeedInSmwUnits);

            // if speed is below min walking speed, set it to 0
            if (Mathf.Abs(newXSpeedInSmwUnits) < minWalkingSpeed)
            {
                newXSpeedInSmwUnits = 0;
            }
        }
        else
        {
            // We're in the air, so momentum calculations instead
            newXSpeedInSmwUnits = CalculateNewXAirSpeed(currentXSpeedInSmwUnits);
        }

        // Apply SMW gravity to vertical speed (in SMW units)
        var newYSpeedInSmwUnits = currentYSpeedInSmwUnits - (isJumping ? lastDampenedGravity : lastGravity);
        // clamp to max speed (0x04800)
        newYSpeedInSmwUnits = Mathf.Clamp(newYSpeedInSmwUnits, -HexToFloat(0x04800), HexToFloat(0x05000));

        // Convert back to Unity's m/s
        velocity.x = newXSpeedInSmwUnits * SmwFramerate;
        velocity.y = newYSpeedInSmwUnits * SmwFramerate;

        // Apply the calculated velocity
        _rb.linearVelocity = velocity;
    }

    private float CalculateNewXGroundSpeed(float currentSpeed)
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

        // if not moving, set the speed to min walking speed
        if (currentSpeed == 0)
        {
            newSpeed = Mathf.Sign(xAxisInput) * minWalkingSpeed;
        }

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
            newSpeed += xAxisInput * (isRunning || _runningCountdown > 0 ? runningAcceleration : walkingAcceleration);
        }

        // Clamp the speed to the max walking speed or max running speed (ensure it respects direction)
        var maxSpeed = isRunning || _runningCountdown > 0 ? maxRunningSpeed : maxWalkingSpeed;
        newSpeed = Mathf.Clamp(newSpeed, -maxSpeed, maxSpeed);

        return newSpeed;
    }

    private float CalculateNewXAirSpeed(float currentSpeed)
    {
        // If no input is given, don't change anything
        if (xAxisInput == 0)
        {
            return currentSpeed;
        }

        // Calculate deceleration based on current speed
        var deceleration = currentSpeed switch
        {
            < 0x01900 => momentumDecelerationSmall,
            < 0x02500 => momentumDecelerationMedium,
            _ => momentumDecelerationLarge
        };

        // Calculate acceleration and max speed
        var acceleration = currentSpeed < 0x01900 ? momentumAccelerationSmall : momentumAccelerationLarge;
        var maxAirSpeed = initialXSpeedWhenJumping < HexToFloat(0x01900) ? maxAirSpeedSmall : maxAirSpeedLarge;

        // Input is against current direction - decelerate
        if (Math.Sign(xAxisInput) != Math.Sign(currentSpeed))
        {
            return currentSpeed > 0
                ? Mathf.Max(currentSpeed - deceleration, 0)
                : Mathf.Min(currentSpeed + deceleration, 0);
        }

        // Input is in the same direction - accelerate (if below max speed)
        currentSpeed += xAxisInput * acceleration;

        // Clamp to max speed
        return Mathf.Clamp(currentSpeed, -maxAirSpeed, maxAirSpeed);
    }

    private void Jump()
    {
        // set the initial speed when jumping
        initialXSpeedWhenJumping = _rb.linearVelocity.x / SmwFramerate;

        // Figure out what speed to apply
        float jumpSpeed;

        if (initialXSpeedWhenJumping < HexToFloat(0x01000))
        {
            jumpSpeed = jumpSpeedSmall;
            lastGravity = normalGravitySmall;
            lastDampenedGravity = dampenedGravitySmall;

            AudioManager.Instance.PlaySFX("jump small");
        }
        else if (initialXSpeedWhenJumping < HexToFloat(0x02500))
        {
            jumpSpeed = jumpSpeedMedium;
            lastGravity = normalGravityMedium;
            lastDampenedGravity = dampenedGravityMedium;

            AudioManager.Instance.PlaySFX("jump small");
        }
        else
        {
            jumpSpeed = jumpSpeedLarge;
            lastGravity = normalGravityLarge;
            lastDampenedGravity = dampenedGravityLarge;

            AudioManager.Instance.PlaySFX("jump super");
        }

        // Apply the jump speed
        var velocityInUnits = jumpSpeed * SmwFramerate;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, velocityInUnits);
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
            _runningCountdown = 10;
        }
    }

    // callback for the jump button with unity's input system
    public void OnJump(InputAction.CallbackContext context)
    {
        var wasJumping = isJumping;
        isJumping = context.ReadValue<float>() > 0;

        // only jump if not currently pressing the jump button and if not falling
        if (isJumping && !wasJumping && Mathf.Abs(_rb.linearVelocity.y) < 0.05f)
        {
            Jump();
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

// GENERATED AUTOMATICALLY FROM 'Assets/_NoiseHunters/Inputs/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerMap"",
            ""id"": ""e7c0aea5-5300-4c32-9ace-d262fd87d1cd"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""5070f3ad-8cd2-47c4-a720-fb23296c4731"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ThrowAndMagnet"",
                    ""type"": ""Button"",
                    ""id"": ""0015824c-1c26-4edf-97ab-9a643f637784"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""50448dda-978b-47bf-b8ca-793002551617"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StopAim"",
                    ""type"": ""Button"",
                    ""id"": ""a5662b5f-30ea-4ce0-82f0-af271254b2f0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""Value"",
                    ""id"": ""2503583b-56ce-4a42-bc44-13a6c486cf41"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""372d1be6-7b3a-4194-8db8-5df8aa6a8d60"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""StopSprint"",
                    ""type"": ""Button"",
                    ""id"": ""5e4eebe1-c1ad-438f-bed8-3a32db97dedf"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""443b91de-9d56-4d1b-9e92-c00f12c5eff5"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""47ec3fea-3d69-475d-8343-c14ca7ba9b92"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""49768ce9-e8eb-4084-929f-1e3ec62ca0eb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""09e26600-7d58-4cc8-9126-37dc3ce0121a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ee1d7721-2602-437c-ad59-a6e830e8151b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ThrowAndMagnet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c71c507e-8602-4e9e-92f7-f34fc8cf88d9"",
                    ""path"": ""<XInputController>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""ThrowAndMagnet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c4e6dbe-dc24-47e5-9ec9-3211c7695511"",
                    ""path"": ""<DualShockGamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""ThrowAndMagnet"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da6c2ef4-b811-4e43-b6aa-15d463162894"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d5b79fd-2b1a-4602-b506-95e2f365d57d"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8805ad33-08d6-45ae-bc56-3b9a85befc5c"",
                    ""path"": ""<DualShockGamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""208e4ed3-e84f-4966-a213-115402e5c793"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ffdace2c-b1d0-4df5-a349-6b78b6710713"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e141caca-a764-4ff5-9ea6-37d87f88bb23"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""db1aad4c-1bc6-46c5-8204-6b6031810303"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""09043be0-51eb-4f08-8a36-9ca3612155a3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3257234a-5e93-4804-ab9d-565c1ca1ba20"",
                    ""path"": ""<XInputController>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79c2a694-96e5-4ed1-92f3-bfb8ca926d22"",
                    ""path"": ""<DualShockGamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38912916-9297-424a-8b20-60c0aafe4268"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9293d03-7178-4950-be18-f4a601a17178"",
                    ""path"": ""<XInputController>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""68e521fc-1391-4cde-a720-3302c39bc1de"",
                    ""path"": ""<DualShockGamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b793ecc9-e6da-43a3-872e-b8667f28c4c5"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""29175ec5-fd4f-45f4-b8db-c5cdde53d204"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97de0e1f-a910-4460-a8ec-0c5f4156aee7"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ad36ca7-00ed-4f33-9c68-37e8f6865ef9"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c55a856c-3514-4c57-a689-2f299050e733"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b56e008-b431-454f-b619-10628bf23e3d"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6a8d405-4f6c-4aba-973e-4b04a3b3f78d"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a196fcb0-300c-444e-9854-94c4af4ff7ef"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1569a5d-e05a-43c3-ae99-94037bb42828"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef7fa410-0daa-4555-a3df-efcca69cbcb6"",
                    ""path"": ""<DualShockGamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bcfc895-6659-4528-b1ea-b97afb0de34d"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c23c7ad1-0e08-46fa-aa6b-78ec65e88706"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87671f4e-0e60-461b-b986-fc23b6b1dc60"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a3ae57bd-f8a2-4cb2-89a3-986cb3f93230"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9adeff12-8bcf-4963-a492-048b27b04ddb"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""562e9822-ff69-4081-b1c6-8c32b752a48a"",
                    ""path"": ""<XInputController>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50bf7b54-8a4d-4243-b468-640e74003163"",
                    ""path"": ""<DualShockGamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7bc23ad-8aa7-4723-89da-3e50ed9a0175"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""StopAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9126b569-0448-4b9c-8a90-1e6c255d2dff"",
                    ""path"": ""<XInputController>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""StopAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43be87fc-82f6-4bc8-8b24-132ac9cccb59"",
                    ""path"": ""<DualShockGamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""StopAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""334f2258-5647-4937-8d5e-2b773490386d"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""StopSprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9248b53-5f86-4afa-938d-188bba25908a"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""StopSprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4d152f9f-27e5-4246-b206-91845d00e7c1"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""StopSprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""VideoMap"",
            ""id"": ""2e9d925e-75e1-4d27-93c7-6406916c7972"",
            ""actions"": [
                {
                    ""name"": ""StartOmit"",
                    ""type"": ""Button"",
                    ""id"": ""15fc8c82-f57e-4676-b45e-712ef4779ffd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""EndOmit"",
                    ""type"": ""Button"",
                    ""id"": ""6ee96506-c880-4a10-933c-23db6701611a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""52b8ce4c-6216-4a5f-bad0-2593cf5125ae"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""StartOmit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15dff5c0-5866-4a93-8a97-45c301b81e66"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""StartOmit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb8eb793-878a-4abe-9cdd-6f80d1c7ade1"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""StartOmit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50697834-b6e1-4f0a-be75-058be301ff14"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""EndOmit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2cb0807d-8c3d-478e-af1a-cc4c83ccdaa4"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""EndOmit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8269c56d-6a11-407e-92e6-2f7fbd5dcfac"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""EndOmit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UIMap"",
            ""id"": ""a29ccfac-4e0b-4e60-bc9e-b142d4a4edf6"",
            ""actions"": [
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a46b400f-4063-493e-8fa7-711d39cfad9c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1e3372f8-2312-410f-be8b-d51dd743cf5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""29ed7d54-6628-452c-906b-79e2006b7502"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""972ac8d2-4eeb-4286-a6f1-c2b11cf4c4fe"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""473c0275-fcb0-4fb1-932d-23dd7f5dde2b"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""325ff514-28a0-4c43-ba7c-f92d82a53aa0"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""b0c1e964-78f8-41db-9978-c087c900adf3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""8daffbba-9d4c-4b54-b54a-8caa855492d3"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b6af8138-93ca-4ff7-b9b5-9623266b5edc"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9138bee-1094-41b7-a9df-2ebead87f8a7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c075deeb-1955-43da-9221-a23efbbdde83"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""316a3106-8d44-4eac-a56d-fbc975bc8a24"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""042d2db8-3770-4fd2-a0ef-919e91f41e7b"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""3f9d165f-1068-4d64-acdb-a0b2fa96938a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1222131b-fe90-41a8-9e36-6ac92289fa3f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""ff776b41-b08c-4a4e-ad4d-23c5092e2a34"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b467e00d-2c55-4423-8e15-c8362e7f5d34"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2b1d9bdd-a86f-471c-869a-9963fcfebfd4"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""323c9c4c-e5d8-440a-a561-36bee8536f48"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""017494d0-3a34-41fa-a352-2a60603fb37f"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8a1868c3-c8af-4094-bc74-bf975686217a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5720efc6-223a-4e42-9f3e-082c2a92b4bf"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Xbox"",
                    ""id"": ""a2369807-40b6-4471-9267-add6f7e24fc7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8f25f346-40cb-415b-b7b6-6779591093f7"",
                    ""path"": ""<XInputController>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7c63e196-5297-4878-81d6-37960713f007"",
                    ""path"": ""<XInputController>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c75c0985-670e-426c-881a-1ab4ada77327"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9eedd5e7-1453-49df-8955-887e8078218a"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5e866193-c2f0-4c94-a316-0f9a8f79a0c7"",
                    ""path"": ""<XInputController>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""PlayStation"",
                    ""id"": ""c6f1fdb6-4864-4d07-a43d-499284dd13c4"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9cef48d3-9e05-4a11-a7e8-7d0def41b7cd"",
                    ""path"": ""<DualShockGamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""27a41366-ea63-4732-9c0c-7b8d892c2b2a"",
                    ""path"": ""<DualShockGamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b0805839-0d8e-48fe-b416-c478f99f1992"",
                    ""path"": ""<DualShockGamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""39bc953d-de3a-47ee-92fd-8c25b330d089"",
                    ""path"": ""<DualShockGamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3ade5ce4-0cf7-4e55-851b-af71eeecb82f"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2109d787-72ef-4873-a18c-88c902926007"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14f60de4-41e4-4751-914b-d1e4da829510"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5a6db8a7-9972-41af-9e30-ee30d43cb57f"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eda6310a-9baa-44f8-a6d8-4a3d5cd3623b"",
                    ""path"": ""<DualShockGamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53ead4b7-594a-45bf-a1f5-40a1fdd8bb30"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""062d1e0b-f750-4aa6-ab6f-f1c1cb7b96d7"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard & Mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ca61d47-34f4-46bd-9e9e-4dddf5305d79"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b81a1c7-5405-43cb-9d6d-356bed654d48"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Xbox"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""66e63537-3bbf-40aa-8bd6-bd2aea54f5ea"",
                    ""path"": ""<DualShockGamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""46f8607e-2006-4137-8ee3-d901f34bfcc4"",
                    ""path"": ""<DualShockGamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PlayStation"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard & Mouse"",
            ""bindingGroup"": ""Keyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Xbox"",
            ""bindingGroup"": ""Xbox"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PlayStation"",
            ""bindingGroup"": ""PlayStation"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerMap
        m_PlayerMap = asset.FindActionMap("PlayerMap", throwIfNotFound: true);
        m_PlayerMap_Movement = m_PlayerMap.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMap_ThrowAndMagnet = m_PlayerMap.FindAction("ThrowAndMagnet", throwIfNotFound: true);
        m_PlayerMap_Aim = m_PlayerMap.FindAction("Aim", throwIfNotFound: true);
        m_PlayerMap_StopAim = m_PlayerMap.FindAction("StopAim", throwIfNotFound: true);
        m_PlayerMap_CameraMovement = m_PlayerMap.FindAction("CameraMovement", throwIfNotFound: true);
        m_PlayerMap_Sprint = m_PlayerMap.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerMap_StopSprint = m_PlayerMap.FindAction("StopSprint", throwIfNotFound: true);
        m_PlayerMap_Crouch = m_PlayerMap.FindAction("Crouch", throwIfNotFound: true);
        m_PlayerMap_Interact = m_PlayerMap.FindAction("Interact", throwIfNotFound: true);
        m_PlayerMap_Pause = m_PlayerMap.FindAction("Pause", throwIfNotFound: true);
        m_PlayerMap_Select = m_PlayerMap.FindAction("Select", throwIfNotFound: true);
        // VideoMap
        m_VideoMap = asset.FindActionMap("VideoMap", throwIfNotFound: true);
        m_VideoMap_StartOmit = m_VideoMap.FindAction("StartOmit", throwIfNotFound: true);
        m_VideoMap_EndOmit = m_VideoMap.FindAction("EndOmit", throwIfNotFound: true);
        // UIMap
        m_UIMap = asset.FindActionMap("UIMap", throwIfNotFound: true);
        m_UIMap_Point = m_UIMap.FindAction("Point", throwIfNotFound: true);
        m_UIMap_LeftClick = m_UIMap.FindAction("LeftClick", throwIfNotFound: true);
        m_UIMap_MiddleClick = m_UIMap.FindAction("MiddleClick", throwIfNotFound: true);
        m_UIMap_RightClick = m_UIMap.FindAction("RightClick", throwIfNotFound: true);
        m_UIMap_ScrollWheel = m_UIMap.FindAction("ScrollWheel", throwIfNotFound: true);
        m_UIMap_Move = m_UIMap.FindAction("Move", throwIfNotFound: true);
        m_UIMap_Submit = m_UIMap.FindAction("Submit", throwIfNotFound: true);
        m_UIMap_Cancel = m_UIMap.FindAction("Cancel", throwIfNotFound: true);
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

    // PlayerMap
    private readonly InputActionMap m_PlayerMap;
    private IPlayerMapActions m_PlayerMapActionsCallbackInterface;
    private readonly InputAction m_PlayerMap_Movement;
    private readonly InputAction m_PlayerMap_ThrowAndMagnet;
    private readonly InputAction m_PlayerMap_Aim;
    private readonly InputAction m_PlayerMap_StopAim;
    private readonly InputAction m_PlayerMap_CameraMovement;
    private readonly InputAction m_PlayerMap_Sprint;
    private readonly InputAction m_PlayerMap_StopSprint;
    private readonly InputAction m_PlayerMap_Crouch;
    private readonly InputAction m_PlayerMap_Interact;
    private readonly InputAction m_PlayerMap_Pause;
    private readonly InputAction m_PlayerMap_Select;
    public struct PlayerMapActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMap_Movement;
        public InputAction @ThrowAndMagnet => m_Wrapper.m_PlayerMap_ThrowAndMagnet;
        public InputAction @Aim => m_Wrapper.m_PlayerMap_Aim;
        public InputAction @StopAim => m_Wrapper.m_PlayerMap_StopAim;
        public InputAction @CameraMovement => m_Wrapper.m_PlayerMap_CameraMovement;
        public InputAction @Sprint => m_Wrapper.m_PlayerMap_Sprint;
        public InputAction @StopSprint => m_Wrapper.m_PlayerMap_StopSprint;
        public InputAction @Crouch => m_Wrapper.m_PlayerMap_Crouch;
        public InputAction @Interact => m_Wrapper.m_PlayerMap_Interact;
        public InputAction @Pause => m_Wrapper.m_PlayerMap_Pause;
        public InputAction @Select => m_Wrapper.m_PlayerMap_Select;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMapActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMapActions instance)
        {
            if (m_Wrapper.m_PlayerMapActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMovement;
                @ThrowAndMagnet.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnThrowAndMagnet;
                @ThrowAndMagnet.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnThrowAndMagnet;
                @ThrowAndMagnet.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnThrowAndMagnet;
                @Aim.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnAim;
                @StopAim.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnStopAim;
                @StopAim.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnStopAim;
                @StopAim.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnStopAim;
                @CameraMovement.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnCameraMovement;
                @Sprint.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSprint;
                @StopSprint.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnStopSprint;
                @StopSprint.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnStopSprint;
                @StopSprint.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnStopSprint;
                @Crouch.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnCrouch;
                @Interact.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnInteract;
                @Pause.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnPause;
                @Select.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_PlayerMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @ThrowAndMagnet.started += instance.OnThrowAndMagnet;
                @ThrowAndMagnet.performed += instance.OnThrowAndMagnet;
                @ThrowAndMagnet.canceled += instance.OnThrowAndMagnet;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @StopAim.started += instance.OnStopAim;
                @StopAim.performed += instance.OnStopAim;
                @StopAim.canceled += instance.OnStopAim;
                @CameraMovement.started += instance.OnCameraMovement;
                @CameraMovement.performed += instance.OnCameraMovement;
                @CameraMovement.canceled += instance.OnCameraMovement;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @StopSprint.started += instance.OnStopSprint;
                @StopSprint.performed += instance.OnStopSprint;
                @StopSprint.canceled += instance.OnStopSprint;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
            }
        }
    }
    public PlayerMapActions @PlayerMap => new PlayerMapActions(this);

    // VideoMap
    private readonly InputActionMap m_VideoMap;
    private IVideoMapActions m_VideoMapActionsCallbackInterface;
    private readonly InputAction m_VideoMap_StartOmit;
    private readonly InputAction m_VideoMap_EndOmit;
    public struct VideoMapActions
    {
        private @PlayerControls m_Wrapper;
        public VideoMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @StartOmit => m_Wrapper.m_VideoMap_StartOmit;
        public InputAction @EndOmit => m_Wrapper.m_VideoMap_EndOmit;
        public InputActionMap Get() { return m_Wrapper.m_VideoMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(VideoMapActions set) { return set.Get(); }
        public void SetCallbacks(IVideoMapActions instance)
        {
            if (m_Wrapper.m_VideoMapActionsCallbackInterface != null)
            {
                @StartOmit.started -= m_Wrapper.m_VideoMapActionsCallbackInterface.OnStartOmit;
                @StartOmit.performed -= m_Wrapper.m_VideoMapActionsCallbackInterface.OnStartOmit;
                @StartOmit.canceled -= m_Wrapper.m_VideoMapActionsCallbackInterface.OnStartOmit;
                @EndOmit.started -= m_Wrapper.m_VideoMapActionsCallbackInterface.OnEndOmit;
                @EndOmit.performed -= m_Wrapper.m_VideoMapActionsCallbackInterface.OnEndOmit;
                @EndOmit.canceled -= m_Wrapper.m_VideoMapActionsCallbackInterface.OnEndOmit;
            }
            m_Wrapper.m_VideoMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @StartOmit.started += instance.OnStartOmit;
                @StartOmit.performed += instance.OnStartOmit;
                @StartOmit.canceled += instance.OnStartOmit;
                @EndOmit.started += instance.OnEndOmit;
                @EndOmit.performed += instance.OnEndOmit;
                @EndOmit.canceled += instance.OnEndOmit;
            }
        }
    }
    public VideoMapActions @VideoMap => new VideoMapActions(this);

    // UIMap
    private readonly InputActionMap m_UIMap;
    private IUIMapActions m_UIMapActionsCallbackInterface;
    private readonly InputAction m_UIMap_Point;
    private readonly InputAction m_UIMap_LeftClick;
    private readonly InputAction m_UIMap_MiddleClick;
    private readonly InputAction m_UIMap_RightClick;
    private readonly InputAction m_UIMap_ScrollWheel;
    private readonly InputAction m_UIMap_Move;
    private readonly InputAction m_UIMap_Submit;
    private readonly InputAction m_UIMap_Cancel;
    public struct UIMapActions
    {
        private @PlayerControls m_Wrapper;
        public UIMapActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Point => m_Wrapper.m_UIMap_Point;
        public InputAction @LeftClick => m_Wrapper.m_UIMap_LeftClick;
        public InputAction @MiddleClick => m_Wrapper.m_UIMap_MiddleClick;
        public InputAction @RightClick => m_Wrapper.m_UIMap_RightClick;
        public InputAction @ScrollWheel => m_Wrapper.m_UIMap_ScrollWheel;
        public InputAction @Move => m_Wrapper.m_UIMap_Move;
        public InputAction @Submit => m_Wrapper.m_UIMap_Submit;
        public InputAction @Cancel => m_Wrapper.m_UIMap_Cancel;
        public InputActionMap Get() { return m_Wrapper.m_UIMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIMapActions set) { return set.Get(); }
        public void SetCallbacks(IUIMapActions instance)
        {
            if (m_Wrapper.m_UIMapActionsCallbackInterface != null)
            {
                @Point.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnPoint;
                @Point.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnPoint;
                @Point.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnPoint;
                @LeftClick.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnLeftClick;
                @MiddleClick.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnMiddleClick;
                @MiddleClick.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnMiddleClick;
                @RightClick.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnRightClick;
                @ScrollWheel.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnScrollWheel;
                @ScrollWheel.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnScrollWheel;
                @Move.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnMove;
                @Submit.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIMapActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIMapActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIMapActionsCallbackInterface.OnCancel;
            }
            m_Wrapper.m_UIMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Point.started += instance.OnPoint;
                @Point.performed += instance.OnPoint;
                @Point.canceled += instance.OnPoint;
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @MiddleClick.started += instance.OnMiddleClick;
                @MiddleClick.performed += instance.OnMiddleClick;
                @MiddleClick.canceled += instance.OnMiddleClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
                @ScrollWheel.started += instance.OnScrollWheel;
                @ScrollWheel.performed += instance.OnScrollWheel;
                @ScrollWheel.canceled += instance.OnScrollWheel;
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
            }
        }
    }
    public UIMapActions @UIMap => new UIMapActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard & Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_XboxSchemeIndex = -1;
    public InputControlScheme XboxScheme
    {
        get
        {
            if (m_XboxSchemeIndex == -1) m_XboxSchemeIndex = asset.FindControlSchemeIndex("Xbox");
            return asset.controlSchemes[m_XboxSchemeIndex];
        }
    }
    private int m_PlayStationSchemeIndex = -1;
    public InputControlScheme PlayStationScheme
    {
        get
        {
            if (m_PlayStationSchemeIndex == -1) m_PlayStationSchemeIndex = asset.FindControlSchemeIndex("PlayStation");
            return asset.controlSchemes[m_PlayStationSchemeIndex];
        }
    }
    public interface IPlayerMapActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnThrowAndMagnet(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnStopAim(InputAction.CallbackContext context);
        void OnCameraMovement(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnStopSprint(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
    public interface IVideoMapActions
    {
        void OnStartOmit(InputAction.CallbackContext context);
        void OnEndOmit(InputAction.CallbackContext context);
    }
    public interface IUIMapActions
    {
        void OnPoint(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnMiddleClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
        void OnScrollWheel(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
    }
}

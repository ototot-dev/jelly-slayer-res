using System;
using PampelGames.Shared.Utility;
using UnityEngine;
using UnityEngine.UIElements;

namespace PampelGames.BloodFactory.Demo
{
    public class DemoUI : MonoBehaviour
    {
        public UIDocument uiDocument;
        public DemoExecute demoExecute;
        public DemoCameraController demoCameraController;

        private VisualElement root;

        private Label currentObject;

        private Button executeMeshDecalsPrevious;
        private Button executeMeshDecalsNext;
        private Button executeMeshDecalsRepeat;
        private Button executeBloodSplashesPrevious;
        private Button executeBloodSplashesNext;
        private Button executeBloodSplashesRepeat;
        private Button executeBloodSplattersPrevious;
        private Button executeBloodSplattersNext;
        private Button executeBloodSplattersRepeat;

        private Slider cameraZoom;
        private Slider cameraRotateY;

        private void Awake()
        {
            GetElements();
            RegisterButtonEvents();
            RegisterCameraEvents();

            currentObject.text = "";
        }

        private void GetElements()
        {
            root = uiDocument.rootVisualElement;

            currentObject = root.Q<Label>("currentObject");
                
            executeMeshDecalsPrevious = root.Q<Button>("executeMeshDecalsPrevious");
            executeMeshDecalsNext = root.Q<Button>("executeMeshDecalsNext");
            executeMeshDecalsRepeat = root.Q<Button>("executeMeshDecalsRepeat");
            executeBloodSplashesPrevious = root.Q<Button>("executeBloodSplashesPrevious");
            executeBloodSplashesNext = root.Q<Button>("executeBloodSplashesNext");
            executeBloodSplashesRepeat = root.Q<Button>("executeBloodSplashesRepeat");
            executeBloodSplattersPrevious = root.Q<Button>("executeBloodSplattersPrevious");
            executeBloodSplattersNext = root.Q<Button>("executeBloodSplattersNext");
            executeBloodSplattersRepeat = root.Q<Button>("executeBloodSplattersRepeat");

            cameraZoom = root.Q<Slider>("cameraZoom");
            cameraRotateY = root.Q<Slider>("cameraRotateY");
        }

        private void RegisterButtonEvents()
        {
            executeMeshDecalsPrevious.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteMeshDecalPrevious());
            };

            executeMeshDecalsNext.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteMeshDecalNext());
            };

            executeMeshDecalsRepeat.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteMeshDecalRepeat());
            };

            executeBloodSplashesPrevious.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteBloodSplashPrevious());
            };

            executeBloodSplashesNext.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteBloodSplashNext());
            };

            executeBloodSplashesRepeat.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteBloodSplashRepeat());
            };

            executeBloodSplattersPrevious.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteBloodSplatterPrevious());
            };

            executeBloodSplattersNext.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteBloodSplatterNext());
            };

            executeBloodSplattersRepeat.clicked += () =>
            {
                InitializeExecutable(demoExecute.ExecuteBloodSplatterRepeat());
            };
        }

        private void InitializeExecutable(PGIExecutable pgiExecutable)
        {
            currentObject.text = ((Component) pgiExecutable).gameObject.name;
        }
        
        private void RegisterCameraEvents()
        {
            demoCameraController.ZoomCamera(0.5f);
            cameraZoom.SetValueWithoutNotify(0.5f);
            cameraZoom.RegisterValueChangedCallback(evt =>
            {
                demoCameraController.ZoomCamera(cameraZoom.value);
            });

            demoCameraController.RotateAroundY(0f);
            cameraRotateY.SetValueWithoutNotify(0f);
            cameraRotateY.RegisterValueChangedCallback(evt =>
            {
                demoCameraController.RotateAroundY(cameraRotateY.value);
            });
        }
    }
}
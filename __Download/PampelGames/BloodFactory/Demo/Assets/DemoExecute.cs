using System;
using System.Collections.Generic;
using PampelGames.Shared.Utility;
using UnityEngine;

namespace PampelGames.BloodFactory.Demo
{
    public class DemoExecute : MonoBehaviour
    {
        public GameObject meshDecalParent;
        public GameObject bloodSplashParent;
        public GameObject bloodSplatterParent;

        private readonly List<PGIExecutable> meshDecals = new();
        private readonly List<PGIExecutable> bloodSplashes = new();
        private readonly List<PGIExecutable> bloodSplatters = new();

        private int meshDecalIndex = -1;
        private int bloodSplashIndex = -1;
        private int bloodSplatterIndex = -1;
        

        private void Awake()
        {
            foreach (Transform child in meshDecalParent.transform)
            {
                var component = child.GetComponent<PGIExecutable>();
                if (component != null) meshDecals.Add(component);
            }

            foreach (Transform child in bloodSplashParent.transform)
            {
                var component = child.GetComponent<PGIExecutable>();
                if (component != null) bloodSplashes.Add(component);
            }

            foreach (Transform child in bloodSplatterParent.transform)
            {
                var component = child.GetComponent<PGIExecutable>();
                if (component != null) bloodSplatters.Add(component);
            }
            
            DeactivateObjects();
        }

        /********************************************************************************************************************************/
        /********************************************************************************************************************************/
        
        public PGIExecutable ExecuteMeshDecalPrevious()
        {
            DeactivateObjects();
            
            meshDecalIndex--;
            if (meshDecalIndex < 0) meshDecalIndex = meshDecals.Count - 1;
            var pgIExecutable = meshDecals[meshDecalIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }
        
        public PGIExecutable ExecuteMeshDecalNext()
        {
            DeactivateObjects();
    
            meshDecalIndex++;
            if (meshDecalIndex >= meshDecals.Count) meshDecalIndex = 0;
            var pgIExecutable = meshDecals[meshDecalIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }

        public PGIExecutable ExecuteMeshDecalRepeat()
        {
            DeactivateObjects();
    
            if (meshDecalIndex < 0 || meshDecalIndex >= meshDecals.Count) meshDecalIndex = 0;
            var pgIExecutable = meshDecals[meshDecalIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }

        /********************************************************************************************************************************/

        public PGIExecutable ExecuteBloodSplashPrevious()
        {
            DeactivateObjects();

            bloodSplashIndex--;
            if (bloodSplashIndex < 0) bloodSplashIndex = bloodSplashes.Count - 1;
            var pgIExecutable = bloodSplashes[bloodSplashIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }

        public PGIExecutable ExecuteBloodSplashNext()
        {
            DeactivateObjects();

            bloodSplashIndex++;
            if (bloodSplashIndex >= bloodSplashes.Count) bloodSplashIndex = 0;
            var pgIExecutable = bloodSplashes[bloodSplashIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }

        public PGIExecutable ExecuteBloodSplashRepeat()
        {
            DeactivateObjects();

            if (bloodSplashIndex < 0 || bloodSplashIndex >= bloodSplashes.Count) bloodSplashIndex = 0;
            var pgIExecutable = bloodSplashes[bloodSplashIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }


        /********************************************************************************************************************************/

        public PGIExecutable ExecuteBloodSplatterPrevious()
        {
            DeactivateObjects();

            bloodSplatterIndex--;
            if (bloodSplatterIndex < 0) bloodSplatterIndex = bloodSplatters.Count - 1;
            var pgIExecutable = bloodSplatters[bloodSplatterIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }

        public PGIExecutable ExecuteBloodSplatterNext()
        {
            DeactivateObjects();

            bloodSplatterIndex++;
            if (bloodSplatterIndex >= bloodSplatters.Count) bloodSplatterIndex = 0;
            var pgIExecutable = bloodSplatters[bloodSplatterIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }

        public PGIExecutable ExecuteBloodSplatterRepeat()
        {
            DeactivateObjects();

            if (bloodSplatterIndex < 0|| bloodSplatterIndex >= bloodSplatters.Count) bloodSplatterIndex = 0;
            var pgIExecutable =  bloodSplatters[bloodSplatterIndex];
            ExecutePGIExecutable(pgIExecutable);
            return pgIExecutable;
        }
    

    
        /********************************************************************************************************************************/
        /********************************************************************************************************************************/
        
        private void DeactivateObjects()
        {
            foreach (var obj in meshDecals) if (obj is Component component) component.gameObject.SetActive(false);
            foreach (var obj in bloodSplashes) if (obj is Component component) component.gameObject.SetActive(false);
            foreach (var obj in bloodSplatters) if (obj is Component component) component.gameObject.SetActive(false);
        }

        private void ExecutePGIExecutable(PGIExecutable pgiExecutable)
        {
            ((Component) pgiExecutable).gameObject.SetActive(true);
            pgiExecutable.Execute();
        }
    }
}
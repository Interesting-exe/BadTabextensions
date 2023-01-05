using System.Collections;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace bad_tabextensions
{
    public class Main : MelonMod
    {
        private static GridLayoutGroup _gridLayoutGroup = null;
        private static GameObject _horizontalLayoutGroup;
        private static int _tabs;
        
        public override void OnInitializeMelon()
        {
            MelonPreferences_Category category = MelonPreferences.CreateCategory("Bad Tabextensions");
            category.CreateEntry("TabsPerRow", 7);
            _tabs = MelonPreferences.GetEntryValue<int>(category.Identifier, "TabsPerRow");
            MelonPreferences.Save();

            MelonCoroutines.Start(WaitForUI());
            base.OnInitializeMelon();
        }

        private static IEnumerator WaitForUI()
        {
            while(Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>() == null) yield return null;
            while (Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>().transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM") == null) yield return null;

            MelonLogger.Msg("Getting qm");
            GameObject quickMenu = Object.FindObjectOfType<VRC.UI.Elements.QuickMenu>().gameObject;
            MelonLogger.Msg("Getting changing box collider");
            quickMenu.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM").gameObject.GetComponent<BoxCollider>().extents = new Vector3(500, 500, 0.5f);
            MelonLogger.Msg("Destroying horizontal layout group");
            _horizontalLayoutGroup = quickMenu.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup").gameObject;
            Object.Destroy(_horizontalLayoutGroup.GetComponent<HorizontalLayoutGroup>());
            MelonLogger.Msg("Setting up grid layout group");
            MelonCoroutines.Start(AddGrid());
        }

        private static IEnumerator AddGrid()
        {
            while (_gridLayoutGroup == null)
            {
                _gridLayoutGroup = _horizontalLayoutGroup.AddComponent<GridLayoutGroup>();
                yield return new WaitForSeconds(0.25f);
            }
                
            _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayoutGroup.constraintCount = _tabs;
        }
    }
}
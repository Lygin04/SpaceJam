using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Data
{
    [CreateAssetMenu(fileName = "Data_UIPrefab", menuName = "Data/UI/UI Prefab")]
    public class UIDialogController : ScriptableObject
    {
        [SerializeField] private List<UIDialogModel> _uiDialogModels = new List<UIDialogModel>();

        public UIDialogModel GetUIDialogModel(UIDialogType uiDialogType)
        {
            UIDialogModel firstOrDefault =
                _uiDialogModels.FirstOrDefault(dialog => dialog.UIDialogType == uiDialogType);
            if (firstOrDefault == null)
            {
                Debug.LogError($"Dialog of type {uiDialogType} not registreated");
            }

            return firstOrDefault;
        }

        public List<UIDialogModel> UIDialogModels => _uiDialogModels;
    }
}
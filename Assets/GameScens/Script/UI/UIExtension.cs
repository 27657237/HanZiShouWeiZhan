using GameFramework.DataTable;
using GameFramework.Procedure;
using GameFramework.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public static class UIExtension 
{
    public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup, float alpha, float duration)
    {
        float time = 0f;
        float originalAlpha = canvasGroup.alpha;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        canvasGroup.alpha = alpha;
    }

    public static IEnumerator SmoothValue(this Slider slider, float value, float duration)
    {
        float time = 0f;
        float originalValue = slider.value;
        while (time < duration)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(originalValue, value, time / duration);
            yield return new WaitForEndOfFrame();
        }

        slider.value = value;
    }


    public static void CloseUIForm(this UIComponent uiComponent, UIFormLogic uiForm)
    {
        uiComponent.CloseUIForm(uiForm.UIForm);
    }

    public static int? OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, object userData = null)
    {
        UIConfigItem Temp = GameEntry.UIConfigTable.GetConfigById(uiFormId);
        if (!Temp.AllowMultiInstance)
        {
            if (GameEntry.UI.IsLoadingUIForm(Temp.AssetName))
            {
                return null;
            }

            if (GameEntry.UI.HasUIForm(Temp.AssetName))
            {
                return null;
            }
           
        }
         return uiComponent.OpenUIForm(Temp.AssetName, Temp.UIGroupName);
        
        
    }


    
}

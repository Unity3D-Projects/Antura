﻿using Antura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Utilities
{
    public class LanguageSelectorBtn : MonoBehaviour
    {

        void Start()
        {
            bool isVisible = EditionConfig.I.SupportedNativeLanguages.Length > 1;

            gameObject.SetActive(isVisible);
        }

    }
}
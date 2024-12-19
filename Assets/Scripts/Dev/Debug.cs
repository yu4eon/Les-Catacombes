﻿// Auteur: Y. Bourdel
// Hiver 2021 TIM

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// Classe Utilitaire statique Debug 
/// Version 3
///
/// Remplace la fonction Debug de base de Unity
/// Utilisée pour rendre plus clair l'affichage
/// de messages Debug.Log() au tableau par le prof...
///
/// Simplement mettre cette classe dans le dossier Assets
/// d'un projet et le Debug.Log de base sera remplacé par
/// les fonctionnalités de cette classe.
///
/// </summary>

/*
Exemples d'utilisation:
-----------------------
Debug.size = 18; // La taille d'affichage par défault est réglée à 18
Debug.color = Color.green; // La couleur par défaut est réglée à rouge
Debug.Log("Bonjour"); // Affiche Bonjour, selon la taille et la couleur par défaut
Debug.Log("Salut","green"); // Affiche Bonjour, selon la taille par défaut et en vert
Debug.Log("Bye",Color.red); // Affiche Bye, selon la taille par défaut et en rouge
Debug.Log("Test","#0000FF"); // Affiche Test, selon la taille par défaut et en bleu
Debug.Trace("Allo",true,345); // Affiche   Allo  true   345, selon la taille et la couleur par défaut
Debug.active=false; // Désactive tous les Debug.Log
*/

// ----------------------------------------------------------
// IMPORTANT:
// Pour un affichage de taille 18, régler dans le burger
// de la console: Log entry> 5 lines
// ----------------------------------------------------------

public static class Debug
{
    private static int _taille = 12; //  Taille de police pour la console
    private static Color _defaultColor = Color.white; // couleur du texte par défaut
    private static int _wrap = 45; //  Nombre de caractères pour wordwrap

    private static string _defaultHex; // couleur du texte par défaut, en hexadécimal

    // Constructeur statique
    static Debug()
    {
        _defaultHex = "#" + ColorUtility.ToHtmlStringRGB(_defaultColor);
    }

    // -------------------------------------------
    // Setters
    // -------------------------------------------

    /// <summary>
    /// Activer ou désactiver tous les messages Debug
    /// </summary>
    public static bool active
    {
        set { UnityEngine.Debug.unityLogger.logEnabled = value; }
    }

    /// <summary>
    /// Taille de la police dans la console
    /// </summary>
    public static int size
    {
        set { _taille = Mathf.Max(1, value); }
    }

    /// <summary>
    /// Couleur du texte
    /// </summary>
    public static Color color
    {
        set
        {
            _defaultColor = value;
            _defaultHex = "#" + ColorUtility.ToHtmlStringRGB(_defaultColor);
        }
    }

    /// <summary>
    /// Nombre de caractères pour le wordwrap
    /// </summary>
    public static int wrap
    {
        set { _wrap = Mathf.Max(1, value); }
    }

    // -------------------------------------------
    // Méthodes publiques
    // -------------------------------------------

    /// <summary>
    /// Affiche un message console de grande taille
    /// </summary>
    /// <param name="obj">objet à afficher</param>
    public static void Log(object obj)
    {
        string str = obj.ToString();
        str = Wrap(str, _wrap);
        UnityEngine.Debug.Log(FormatString(str));
    }

    /// <summary>
    /// Affiche un message console
    /// selon la taille spécifiée
    /// </summary>
    /// <param name="obj">objet à afficher</param>
    public static void Log(object obj, int size)
    {
        _taille=size;
        string str = obj.ToString();
        str = Wrap(str, _wrap);
        UnityEngine.Debug.Log(FormatString(str));
    }

    /// <summary>
    /// Affiche un message console de grande taille
    /// pouvant accepter une couleur rgb
    /// </summary>
    /// <param name="obj">objet à afficher</param>
    /// <param name="color">couleur rgb (defaut: null)</param>
    /// <param name="size">taille de police (optionnel)</param>
    public static void Log(object obj, Color color, int size = 0)
    {
        if (size <= 0) size = _taille;
        string str = obj.ToString();
        str = Wrap(str, _wrap);
        string hex = "#" + ColorUtility.ToHtmlStringRGB(color);
        string info = $"<size={size}><color={hex}>{str}</color></size>";
        UnityEngine.Debug.Log(info);
    }

    /// <summary>
    /// Affiche un message console de grande taille
    /// pouvant accepter une couleur HTML sous forme de chaine (nom ou hexadecimal)
    /// (IMPORTANT: Dans le burger de la console, régler "Log entry>5 lines")
    /// Couleurs autorisées: red, cyan, blue, darkblue, lightblue, purple, yellow, lime, 
    /// fuchsia, white, silver, grey, black, orange, brown, maroon, green, olive, navy
    /// teal, aqua, magenta
    /// </summary>
    /// <returns>
    /// La chaine affichée (avec balises richtext)
    /// </returns>
    /// <param name="obj">objet à afficher</param>
    /// <param name="color">nom de couleur Web (defaut: null)</param>
    /// <param name="size">taille de police (optionnel)</param>
    public static string Log(object obj, string color, int size = 0)
    {
        string str = obj.ToString();
        string info;
        str = Wrap(str, _wrap);
        if (size <= 0) size = _taille;
        Color newCol;
        if (ColorUtility.TryParseHtmlString(color, out newCol))
        {
            info = $"<size={size}><color={color}>{str}</color></size>";
        }
        else
        {
            info = FormatString(str);
        }

        UnityEngine.Debug.Log(info);
        return info;
    }

    /// <summary>
    /// Affiche un message console de grande taille 
    /// d'après un nombre indéterminé de paramètre reçus.
    /// Ces paramètres seront convertis en chaines
    /// et affichés séparés par des tabulations
    /// </summary>
    public static void Trace(params object[] data)
    {
        string str = "";
        for (int i = 0; i < data.Length; i++)
        {
            str += data[i].ToString() + " \t";
        }
        UnityEngine.Debug.Log(FormatString(str));
    }
	
	// -------------------------------------------
    // Méthodes privées
    // -------------------------------------------
	
    // Effectue un Wordwrap sur une chaine donnée
    private static string Wrap(string text, int margin)
    {
		if (text.Length == 0) return "";
		int start = 0;
		int end;
		List<string> lines = new List<string>();
		//text = Regex.Replace(text, @"\s", " ").Trim();

		while ((end = start + margin) < text.Length)
		{
			while (text[end] != ' ' && text[end] != ',' &&end > start)
			{
				end -= 1;
			}
           
			if (end == start) end = start + margin;
			
			lines.Add(text.Substring(start, end - start));
			start = end + 1;
		}

		if (start < text.Length)
		{
			lines.Add(text.Substring(start));
		}

		return string.Join("\n", lines);
	}

    // retourne une chaine Richtext de taille et couleur
    // par défaut à partir de la chaine reçue
    private static string FormatString(string str)
    {
        return $"<size={_taille}><color={_defaultHex}>{str}</color></size>";
    }

    // retourne un code de couleur HTML à partir d'un objet Color
    private static string ToHexColor(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }

    // -------------------------------------------
    // Les autres méthodes de Debug sont inchangées
    // -------------------------------------------
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
    public static void DrawLine(Vector3 start, Vector3 end)
    {
        UnityEngine.Debug.DrawLine(start, end);
    }
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        UnityEngine.Debug.DrawLine(start, end, color);
    }
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration);
    }
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }
    public static void LogException(Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }
    public static void LogException(Exception exception, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }
    public static bool isDebugBuild = UnityEngine.Debug.isDebugBuild;
}
using System.Collections;
using UnityEngine;

public class Timer3D : MonoBehaviour
{
    [SerializeField] GameObject number3;
    [SerializeField] GameObject number2;
    [SerializeField] GameObject number1;
    [SerializeField] GameObject goText;
    [SerializeField] GameObject canvas;
    [SerializeField] AudioSource countdownSound;
    [SerializeField] AudioSource goSound;

    [SerializeField] GameObject script1Object; // R�f�rence � l'objet contenant Script1
    [SerializeField] GameObject script2Object; // R�f�rence � l'objet contenant Script2

    private playerselection script1; // R�f�rence au script1
    private OpponentManager script2; // R�f�rence au script2
    private bool isCountingDown = true;

    [SerializeField] AudioClip[] countdownClips; // Tableau des clips audio pour 3, 2, 1 et "Go"

    void Start()
    {
        // On s'assure que les scripts sont correctement affect�s aux variables
        script1 = script1Object.GetComponent<playerselection>();
        script2 = script2Object.GetComponent<OpponentManager>();
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Tableau des objets pour les chiffres du compte � rebours
        GameObject[] numbers = { number3, number2, number1, goText };

        for (int i = 0; i < numbers.Length; i++)
        {
            GameObject number = numbers[i];

            // Joue le son associ� au chiffre
            if (countdownClips != null && i < countdownClips.Length)
            {
                AudioSource.PlayClipAtPoint(countdownClips[i], transform.position);
            }

            // D�sactive tous les autres chiffres avant d'en activer un seul
            foreach (GameObject n in numbers)
            {
                if (n != number)
                    n.SetActive(false);
            }

            yield return StartCoroutine(AnimateNumber(number)); // Animation du nombre
            yield return new WaitForSeconds(1f); // Attente entre chaque nombre
        }

        // Cache le canvas apr�s le compte � rebours
        canvas.SetActive(false);
        isCountingDown = false;

        // Active les objets des scripts apr�s le compte � rebours
        script1Object.SetActive(true);
        script2Object.SetActive(true);

        // Ex�cute les m�thodes des scripts
        script1.Start();
        script2.Start();
    }

    IEnumerator AnimateNumber(GameObject obj)
    {
        obj.SetActive(true);
        countdownSound.Play();

        // Sauvegarde l'�chelle et la couleur de l'objet
        Vector3 originalScale = obj.transform.localScale;
        obj.transform.localScale = Vector3.zero;
        Color originalColor = obj.GetComponent<Renderer>().material.color;
        Color transparentColor = originalColor;
        transparentColor.a = 0f;

        obj.GetComponent<Renderer>().material.color = transparentColor;

        float animationDuration = 1f;
        float timeElapsed = 0f;

        // Animation de l'apparition du nombre
        while (timeElapsed < animationDuration)
        {
            float t = timeElapsed / animationDuration;
            obj.transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, t);
            obj.GetComponent<Renderer>().material.color = Color.Lerp(transparentColor, originalColor, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Finalisation de l'�chelle et de la couleur
        obj.transform.localScale = originalScale;
        obj.GetComponent<Renderer>().material.color = originalColor;
        yield return new WaitForSeconds(1f);
        obj.SetActive(false); // D�sactive l'objet apr�s l'animation
    }
}

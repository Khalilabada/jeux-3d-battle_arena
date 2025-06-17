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

    [SerializeField] GameObject script1Object; // Référence à l'objet contenant Script1
    [SerializeField] GameObject script2Object; // Référence à l'objet contenant Script2

    private playerselection script1; // Référence au script1
    private OpponentManager script2; // Référence au script2
    private bool isCountingDown = true;

    [SerializeField] AudioClip[] countdownClips; // Tableau des clips audio pour 3, 2, 1 et "Go"

    void Start()
    {
        // On s'assure que les scripts sont correctement affectés aux variables
        script1 = script1Object.GetComponent<playerselection>();
        script2 = script2Object.GetComponent<OpponentManager>();
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        // Tableau des objets pour les chiffres du compte à rebours
        GameObject[] numbers = { number3, number2, number1, goText };

        for (int i = 0; i < numbers.Length; i++)
        {
            GameObject number = numbers[i];

            // Joue le son associé au chiffre
            if (countdownClips != null && i < countdownClips.Length)
            {
                AudioSource.PlayClipAtPoint(countdownClips[i], transform.position);
            }

            // Désactive tous les autres chiffres avant d'en activer un seul
            foreach (GameObject n in numbers)
            {
                if (n != number)
                    n.SetActive(false);
            }

            yield return StartCoroutine(AnimateNumber(number)); // Animation du nombre
            yield return new WaitForSeconds(1f); // Attente entre chaque nombre
        }

        // Cache le canvas après le compte à rebours
        canvas.SetActive(false);
        isCountingDown = false;

        // Active les objets des scripts après le compte à rebours
        script1Object.SetActive(true);
        script2Object.SetActive(true);

        // Exécute les méthodes des scripts
        script1.Start();
        script2.Start();
    }

    IEnumerator AnimateNumber(GameObject obj)
    {
        obj.SetActive(true);
        countdownSound.Play();

        // Sauvegarde l'échelle et la couleur de l'objet
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

        // Finalisation de l'échelle et de la couleur
        obj.transform.localScale = originalScale;
        obj.GetComponent<Renderer>().material.color = originalColor;
        yield return new WaitForSeconds(1f);
        obj.SetActive(false); // Désactive l'objet après l'animation
    }
}

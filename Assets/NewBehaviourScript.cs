using UnityEngine;

public class TouchControls : MonoBehaviour
{
    public float rotationSpeed = 10f; // Velocità di rotazione
    public float zoomSpeed = 0.1f;    // Velocità di zoom
    public float smoothFactor = 0.1f; // Fattore di smoothing per la rotazione
    public float resetDuration = 1f;  // Durata dell'animazione di reset

    private Vector2 lastTouchPosition; // Memorizza l'ultima posizione del touch
    private Vector3 initialScale;      // Scala iniziale del modello
    private Quaternion initialRotation; // Rotazione iniziale del modello
    private bool isResetting = false;  // Indica se il modello sta tornando alla configurazione iniziale
    private float resetTimer = 0f;     // Timer per l'animazione di reset

    void Start()
    {
        // Salva la scala e la rotazione iniziali del modello
        initialScale = transform.localScale;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        if (isResetting)
        {
            // Animazione di reset
            resetTimer += Time.deltaTime;
            float t = resetTimer / resetDuration;

            // Interpola la scala e la rotazione verso i valori iniziali
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, t);

            // Termina l'animazione di reset
            if (resetTimer >= resetDuration)
            {
                isResetting = false;
                resetTimer = 0f;
            }
        }
        else
        {
            // Gestisci l'input touch
            if (Input.touchCount == 1) // Un solo dito: rotazione rispetto all'asse Y e Z
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    // Calcola la differenza di movimento
                    Vector2 delta = touch.position - lastTouchPosition;

                    // Ruota il modello lungo l'asse Y (orizzontale) e Z (verticale) con smoothing
                    Quaternion targetRotation = transform.rotation *
                        Quaternion.Euler(0, -delta.x * rotationSpeed * Time.deltaTime, 0) * // Rotazione Y (orizzontale)
                        Quaternion.Euler(0, 0, delta.y * rotationSpeed * Time.deltaTime);   // Rotazione Z (verticale)

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothFactor);

                    // Aggiorna l'ultima posizione del touch
                    lastTouchPosition = touch.position;
                }
            }
            else if (Input.touchCount == 2) // Due dita: zoom
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    // Calcola la distanza tra i due tocchi
                    float previousDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
                    float currentDistance = Vector2.Distance(touch1.position, touch2.position);

                    // Calcola la differenza di distanza
                    float deltaDistance = currentDistance - previousDistance;

                    // Applica lo zoom (scala il modello)
                    transform.localScale += Vector3.one * deltaDistance * zoomSpeed * Time.deltaTime;

                    // Limita lo zoom (opzionale)
                    transform.localScale = Vector3.Max(transform.localScale, new Vector3(0.5f, 0.5f, 0.5f)); // Minimo
                    transform.localScale = Vector3.Min(transform.localScale, new Vector3(2f, 2f, 2f));      // Massimo
                }
            }
            else if (Input.touchCount == 3) // Tre dita: reset alla configurazione iniziale
            {
                // Avvia l'animazione di reset
                isResetting = true;
            }
        }
    }
}
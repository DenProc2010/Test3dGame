using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cameras")]
    public Camera firstPersonCamera;   // прив’яжи сюди камеру очей
    public Camera thirdPersonCamera;   // прив’яжи сюди камеру за спиною

    [Header("Look Settings")]
    public float lookSpeed = 2f;       // чутливість миші
    public float maxUpAngle = 50f;     // наскільки можна дивитися вгору
    public float maxDownAngle = 50f;   // наскільки можна дивитися вниз

    private bool isThirdPerson = false; // початково — перша особа
    private float rotationX = 0f;       // поточний кут по X (вертикаль)

    void Start()
    {
        // Задаємо початковий режим: перша особа
        isThirdPerson = false;
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
        
        // Блокуємо курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1) Перемикання камер
        if (Input.GetKeyDown(KeyCode.V))
        {
            isThirdPerson = !isThirdPerson;
            firstPersonCamera.gameObject.SetActive(!isThirdPerson);
            thirdPersonCamera.gameObject.SetActive(isThirdPerson);
        }

        // 2) Обертання вздовж горизонталі (Y) — тіло гравця
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        transform.Rotate(Vector3.up * mouseX);

        // 3) Обертання по вертикалі (X) — камера
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxDownAngle, maxUpAngle);

        if (isThirdPerson)
        {
            // у третьому персоні — камеру обертаємо відносно локального X 
            thirdPersonCamera.transform.localEulerAngles = new Vector3(rotationX, 0f, 0f);
        }
        else
        {
            // у першому персоні — камеру обертаємо, а тіло вже повернулося вище
            firstPersonCamera.transform.localEulerAngles = new Vector3(rotationX, 0f, 0f);
        }
    }
}

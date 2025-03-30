using DG.Tweening;
using UnityEngine;

public class PhoneInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private CallCenterGame callCenterGame;
    [Header("Ringing Settings")]
    public float ringDuration = 1.0f; // Duration of ringing motion
    public float ringPause = 2.0f;    // Pause before ringing again
    public float ringAngle = 15f;     // Maximum tilt angle
    public float shakeSpeed = 0.1f;   // Speed of each shake

    [SerializeField] private SpriteRenderer phoneSpriteRenderer;
    [SerializeField] private Sprite phoneUp;
    [SerializeField] private Sprite phoneDown;
    [SerializeField] private SpriteRenderer phoneAnswer;
    [SerializeField] private GameObject phoneBubble;
    private Sequence ringSequence;

    public Collider2D colliderInteract;

    [SerializeField] private AudioClip ringAudioClip;
    [SerializeField] private AudioClip talkingAudio;
    private void Awake()
    {
        colliderInteract = GetComponent<Collider2D>();

        colliderInteract.enabled = false;
    }


    private void Start()
    {
        
        //StartRinging();
    }



    public void StartRinging()
    {
        colliderInteract.enabled = true;
        ringSequence = DOTween.Sequence();

        ringSequence.AppendCallback(PlayRingSound);
        // Calculate number of shakes needed to fit the ring duration
        int numShakes = Mathf.FloorToInt(ringDuration / (shakeSpeed * 2));

        for (int i = 0; i < numShakes; i++)
        {
            ringSequence.Append(transform.DORotate(new Vector3(0, 0, ringAngle), shakeSpeed).SetEase(Ease.InOutSine));
            ringSequence.Append(transform.DORotate(new Vector3(0, 0, -ringAngle), shakeSpeed).SetEase(Ease.InOutSine));
        }

        // Ensure it resets back to neutral position before pausing
        ringSequence.Append(transform.DORotate(Vector3.zero, shakeSpeed).SetEase(Ease.OutSine));
        ringSequence.AppendCallback(StopRingSound);
        // Pause before the next ring cycle
        ringSequence.AppendInterval(ringPause);

        // Restart the sequence infinitely
        ringSequence.SetLoops(-1, LoopType.Restart);
    }

    private void PlayRingSound()
    {
        AudioManager.Instance.PlaySound(ringAudioClip);
    }

    private void StopRingSound()
    {
        AudioManager.Instance.StopOneShot(ringAudioClip);
    }

    public void Interact()
    {
        AudioManager.Instance.PlayLoopingSound(talkingAudio, volume:0.5f);
        ringSequence.Kill();
        StopRingSound();
        phoneSpriteRenderer.sprite = phoneDown;
        phoneAnswer.sprite = callCenterGame.GetAnswerSprite();
        phoneAnswer.enabled = true;
        colliderInteract.enabled = false;
        phoneBubble.SetActive(true);
    }

    public void ResetInteractable()
    {
        AudioManager.Instance.StopLoopingSound(talkingAudio);
        ringSequence.Kill();
        StopRingSound();
        phoneSpriteRenderer.sprite = phoneUp;
        phoneAnswer.enabled = false;
        colliderInteract.enabled = false;
        phoneBubble.SetActive(false);
    }
}

using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class InputSystem : ComponentSystem
{
    struct Data
    {
        public readonly int Length;
        public ComponentDataArray<PlayerInput> PlayerInput;
    }

    [Inject] private Data group;

    private Text mCubeCount;
    private Button mClearButton;
    private InputField mCountInput;
    private Button mCreateButton;
    
    private Button mMonoClearButton;
    private InputField mMonoCountInput;
    private Button mMonoCreateButton;
    private Toggle mMonoToggle;

    public void SetupUI()
    {
        #region ECS
        mCubeCount = GameObject.Find("CountText").GetComponent<Text>();
        mClearButton = GameObject.Find("ClearButton").GetComponent<Button>();
        mCountInput = GameObject.Find("InputField").GetComponent<InputField>();
        mCreateButton = GameObject.Find("CreateButton").GetComponent<Button>();

        mClearButton.onClick.AddListener(ECSClear);
        mCreateButton.onClick.AddListener(ECSCreateCubes);
        #endregion

        #region Mono
        mMonoClearButton = GameObject.Find("MonoClearButton").GetComponent<Button>();
        mMonoCountInput = GameObject.Find("MonoInputField").GetComponent<InputField>();
        mMonoCreateButton = GameObject.Find("MonoCreateButton").GetComponent<Button>();
        mMonoToggle = GameObject.Find("MonoToggle").GetComponent<Toggle>();

        mMonoClearButton.onClick.AddListener(ECSWorld.ClearAllMono);
        mMonoCreateButton.onClick.AddListener(CreateCubesMono);
        mMonoCountInput.onValueChanged.AddListener(ECSWorld.ChangeCountMono);
        #endregion
    }

    #region Mono
    private void CreateCubesMono()
    {
        ECSWorld.CreateCubesMono(mMonoToggle.isOn);
    }
    #endregion

    private void ECSClear()
    {
        for(int i=0; i< group.Length; i++)
        {
            SetInput(i, -1);
        }
    }

    private void ECSCreateCubes()
    {
        int count;
        if (int.TryParse(mCountInput.text, out count))
        {
            for (int i = 0; i < group.Length; i++)
            {
                SetInput(i, count);
            }
        }
    }

    public void ClearInput(int index)
    {
        SetInput(index, 0);
    }

    private void SetInput(int index, int count)
    {
        var input = group.PlayerInput[index];
        input.CreateCount = count;
        group.PlayerInput[index] = input;
    }

    protected override void OnUpdate()
    {
        ECSWorld.RotateCubes();
    }
}
using UnityEngine.UI;

public class JumakManagement_Employee : UI_Base_JumakManagement
{
    private enum Buttons
    {
        Dim,
        Village_Button,
        Hall_Button,
        Kitchen_Button,
        Employee_Button,
        Menu_Button,
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.Dim).gameObject.BindEvent(ClosePopUp);

        GetButton((int)Buttons.Village_Button).gameObject.BindEvent(ChangeCategoryToVillage);
        GetButton((int)Buttons.Hall_Button).gameObject.BindEvent(ChangeCategoryToHall);
        GetButton((int)Buttons.Kitchen_Button).gameObject.BindEvent(ChangeCategoryToKitchen);
        GetButton((int)Buttons.Menu_Button).gameObject.BindEvent(ChangeCategoryToMenu);
    }
}

namespace Datenshi.Scripts.Controller {
    public interface IInputProvider {
        
        float GetXInput();
        
        float GetYInput();
        
        bool GetButtonDown(int button);
        
        bool GetButton(int button);
        
        bool GetButtonUp(int button);
    }
}
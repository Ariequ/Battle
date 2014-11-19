using UnityEngine;
namespace Battle
{
    public class SkillAgent
    {
        private Vector3 _position;
        private Vector3 _rotation;
        private SkillAgentStatus _status;
        private string _name;

        #region GET & SET

        public Vector3 Postion
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
            }
        }

        public SkillAgentStatus Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        #endregion

        public void Update(float deltaTime)
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPartDataBase : MonoBehaviour
{

    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;

        public Position(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public Position()
        {

        }

        public void GetPosition(Transform transform)
        {
            Vector3 vector = transform.position;
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }

    [System.Serializable]
    public class Rotation
    {
        public float x;
        public float y;
        public float z;

        public Rotation(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public Rotation()
        {

        }

        public void GetRotation(Transform transform)
        {
            Vector3 vector = transform.eulerAngles;
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }

    [System.Serializable]
    public class Scale
    {
        public float x;
        public float y;
        public float z;

        public Scale(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }

        public Scale()
        {

        }

        public void GetScale(Transform transform)
        {
            Vector3 vector = transform.localScale;
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }

    [System.Serializable]
    public class State
    {
        public Part.Stage stage;

        public State(Part.Stage Stage)
        {
            stage = Stage;
        }

        public State()
        {
        }

        public void GetState(GameObject moving)
        {
            stage = moving.GetComponent<Part>().currentStage;
        }
    }

    [System.Serializable]
    public class Spawning_transform
    {
        public Position position;
        public Rotation rotation;
        public Scale scale;

        public Spawning_transform(Position Position, Rotation Rotation, Scale Scale)
        {
            position = Position;
            rotation = Rotation;
            scale = Scale;
        }
    }

    [System.Serializable]
    public class Placing_transform
    {
        public Position position;
        public Rotation rotation;
        public Scale scale;

        public Placing_transform(Position Position, Rotation Rotation, Scale Scale)
        {
            position = Position;
            rotation = Rotation;
            scale = Scale;
        }
    }

    [System.Serializable]
    public class Moving_Part
    {
        [HideInInspector] public GameObject MovingObj;
        public Placing_transform placing;
        public string Hint;
        public State state;
        public Spawning_transform spawning;
        public string Name;

        public Moving_Part(Placing_transform Placing, Spawning_transform Spawning, State State, string hint, string name)
        {
            placing = Placing;
            Hint = hint;
            state = State;
            spawning = Spawning;
            Name = name;
        }

        public void SetProps(Moving_Part moving_Part, List<Position> positions, List<Rotation> rotations, List<Scale> scale, List<Spawning_transform> spawning, List<Placing_transform> placing, List<State> state)
        {
            positions.Add(moving_Part.placing.position);
            rotations.Add(moving_Part.placing.rotation);
            scale.Add(moving_Part.placing.scale);
            positions.Add(moving_Part.spawning.position);
            rotations.Add(moving_Part.spawning.rotation);
            scale.Add(moving_Part.spawning.scale);
            spawning.Add(moving_Part.spawning);
            placing.Add(moving_Part.placing);
            state.Add(moving_Part.state);
        }


    }

    public List<Position> position = new List<Position>();
    public List<Rotation> rotation = new List<Rotation>();
    public List<Scale> scale = new List<Scale>();
    public List<Spawning_transform> spawning = new List<Spawning_transform>();
    public List<Placing_transform> placing = new List<Placing_transform>();
    public List<State> state = new List<State>();
    public List<Moving_Part> moving_Part = new List<Moving_Part>();


    public void FillArrays(Transform Spawn, Transform Placing, GameObject Moving, string Hint)
    {
        Position Pposition = new Position();
        Pposition.GetPosition(Placing);
        position.Add(Pposition);

        Position Sposition = new Position();
        Sposition.GetPosition(Spawn);
        position.Add(Sposition);

        Rotation Protation = new Rotation();
        Protation.GetRotation(Placing);
        rotation.Add(Protation);

        Rotation Srotation = new Rotation();
        Srotation.GetRotation(Spawn);
        rotation.Add(Srotation);

        Scale Pscale = new Scale();
        Pscale.GetScale(Placing);
        scale.Add(Pscale);

        Scale Sscale = new Scale();
        Sscale.GetScale(Spawn);
        scale.Add(Sscale);

        Placing_transform PlacingT = new Placing_transform(Pposition, Protation, Pscale);
        placing.Add(PlacingT);

        Spawning_transform SpawningT = new Spawning_transform(Sposition, Srotation, Sscale);
        spawning.Add(SpawningT);

        State state_ = new State();
        state_.GetState(Moving);
        state.Add(state_);

        string name = Moving.transform.GetChild(0).name;
        name = name.Replace("(Close)", "");

        Moving_Part _Part = new Moving_Part(PlacingT, SpawningT, state_, Hint, name);
        moving_Part.Add(_Part);
    }

    public void UpdateProps(GameObject Moving)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

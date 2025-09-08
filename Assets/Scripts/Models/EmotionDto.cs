using System;
using System.Collections.Generic;

[Serializable]
public class EmotionDto
{
    public EmotionalState EmotionalState;
    public string MessageUser;
}

[Serializable]
public class EmotionalRoot
{
    public EmotionalNode Emotional; 
}

[Serializable]
public class EmotionalNode
{
    public int StateEmotional;
    public List<SpeakerLine> Speaker;
    public List<Choice> Choices;
}

[Serializable]
public class SpeakerLine
{
    public int id;
    public string Text;
}

[Serializable]
public class Choice
{
    public string Caption;
    public int StateEmotional;
    public string Context;
}
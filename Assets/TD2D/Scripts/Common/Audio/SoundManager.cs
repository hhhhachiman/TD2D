using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static GameObject sound_play_object;//������Ǹ��ڵ�
    static bool is_music_mute = false;//��ŵ�ǰȫ�ֱ��������Ƿ����ı���
    static bool is_effect_mute = false;//��ŵ�ǰ��Ч�Ƿ����ı���

    // url --> AudioSource ӳ��, �������֣���Ч
    static Dictionary<string, AudioSource> musics = null;//���ֱ�
    static Dictionary<string, AudioSource> effects = null;//��Ч��

    public static void init()
    {
        sound_play_object = new GameObject("sound_play_object");//��ʼ�����ڵ�
        sound_play_object.AddComponent<sound_scan>();//���������������ص����ڵ���
        GameObject.DontDestroyOnLoad(sound_play_object);//�����л���ʱ�򲻻�ɾ�����ڵ�


        //��ʼ�����ֱ����Ч��
        musics = new Dictionary<string, AudioSource>();
        effects = new Dictionary<string, AudioSource>();

        // �ӱ����������������
        if (PlayerPrefs.HasKey("music_mute"))//�ж�is_music_mute��û�б����ڱ���
        {
            int value = PlayerPrefs.GetInt("music_mute");
            is_music_mute = (value == 1);//intת��bool�����value==1������true���������false
        }

        // �ӱ����������������
        if (PlayerPrefs.HasKey("effect_mute"))//�ж�is_effect_mute��û�б����ڱ���
        {
            int value = PlayerPrefs.GetInt("effect_mute");
            is_effect_mute = (value == 1);//intת��bool�����value==1������true���������false
        }
    }
    //����ָ���������ֵĽӿ�
    public static void play_music(string url, bool is_loop = true)
    {
        AudioSource audio_source = null;
        if (musics.ContainsKey(url))//�ж��Ƿ��Ѿ��ڱ������ֱ�������
        {
            audio_source = musics[url];//�Ǿ�ֱ�Ӹ�ֵ��ȥ
        }
        else//���Ǿ��½�һ���սڵ㣬�ڵ������½�һ��AudioSource���
        {
            GameObject s = new GameObject(url);//����һ���սڵ�
            s.transform.parent = sound_play_object.transform;//����ڵ㵽������

            audio_source = s.AddComponent<AudioSource>();//�սڵ�������AudioSource
            AudioClip clip = Resources.Load<AudioClip>(url);//�������һ��AudioClip��Դ�ļ�
            audio_source.clip = clip;//���������clip����Ϊclip
            audio_source.loop = is_loop;//�������ѭ������
            audio_source.playOnAwake = true;//�ٴλ���ʱ��������
            audio_source.spatialBlend = 0.0f;//����Ϊ2D����

            musics.Add(url, audio_source);//���뵽���������ֵ��У��´ξͿ���ֱ�Ӹ�ֵ��
        }
        audio_source.mute = is_music_mute;
        audio_source.enabled = true;
        audio_source.Play();//��ʼ����
    }

    //ֹͣ����ָ���������ֵĽӿ�
    public static void stop_music(string url)
    {
        AudioSource audio_source = null;
        if (!musics.ContainsKey(url))//�ж��Ƿ��Ѿ��ڱ������ֱ�������
        {
            return;//û������������־�ֱ�ӷ���
        }
        audio_source = musics[url];//�оͰ�audio_sourceֱ�Ӹ�ֵ��ȥ
        audio_source.Stop();//ֹͣ����
    }

    //ֹͣ�������б������ֵĽӿ�
    public static void stop_all_music()
    {
        foreach (AudioSource s in musics.Values)
        {
            s.Stop();
        }
    }

    //ɾ��ָ���������ֺ����Ľڵ�
    public static void clear_music(string url)
    {
        AudioSource audio_source = null;
        if (!musics.ContainsKey(url))//�ж��Ƿ��Ѿ��ڱ������ֱ�������
        {
            return;//û������������־�ֱ�ӷ���
        }
        audio_source = musics[url];//�оͰ�audio_sourceֱ�Ӹ�ֵ��ȥ
        musics[url] = null;//ָ��audio_source������
        GameObject.Destroy(audio_source.gameObject);//ɾ��������ָ��audio_source����Ľڵ�
    }

    //�л��������־�������
    public static void switch_music()
    {
        // �л���������������״̬
        is_music_mute = !is_music_mute;

        //�ѵ�ǰ�Ƿ���д�뱾��
        int value = (is_music_mute) ? 1 : 0;//boolת��int
        PlayerPrefs.SetInt("music_mute", value);

        // �������б������ֵ�AudioSourceԪ��
        foreach (AudioSource s in musics.Values)
        {
            s.mute = is_music_mute;//����Ϊ��ǰ��״̬
        }
    }

    //���ҵĽ���ľ�����ťҪ��ʾ��ʱ�򣬵�������ʾ�رգ����ǿ�ʼ״̬;
    public static bool music_is_off()
    {
        return is_music_mute;
    }





    //��������ʼ����Ч�Ľӿ�
    //����ָ����Ч�Ľӿ�
    public static void play_effect(string url, bool is_loop = false)
    {
        AudioSource audio_source = null;
        if (effects.ContainsKey(url))//�ж��Ƿ��Ѿ�����Ч��������
        {
            audio_source = effects[url];//�Ǿ�ֱ�Ӹ�ֵ��ȥ
        }
        else//���Ǿ��½�һ���սڵ㣬�ڵ������½�һ��AudioSource���
        {
            GameObject s = new GameObject(url);//����һ���սڵ�
            s.transform.parent = sound_play_object.transform;//����ڵ㵽������

            audio_source = s.AddComponent<AudioSource>();//�սڵ�������AudioSource
            AudioClip clip = Resources.Load<AudioClip>(url);//�������һ��AudioClip��Դ�ļ�
            audio_source.clip = clip;//���������clip����Ϊclip
            audio_source.loop = is_loop;//�������ѭ������
            audio_source.playOnAwake = true;//�ٴλ���ʱ��������
            audio_source.spatialBlend = 0.0f;//����Ϊ2D����

            effects.Add(url, audio_source);//���뵽��Ч�ֵ��У��´ξͿ���ֱ�Ӹ�ֵ��
        }
        audio_source.mute = is_effect_mute;
        audio_source.enabled = true;
        audio_source.Play();//��ʼ����
    }


    //ֹͣ����ָ����Ч�Ľӿ�
    public static void stop_effect(string url)
    {
        AudioSource audio_source = null;
        if (!effects.ContainsKey(url))//�ж��Ƿ��Ѿ�����Ч��������
        {
            return;//û������������־�ֱ�ӷ���
        }
        audio_source = effects[url];//�оͰ�audio_sourceֱ�Ӹ�ֵ��ȥ
        audio_source.Stop();//ֹͣ����
    }

    //ֹͣ����������Ч�Ľӿ�
    public static void stop_all_effect()
    {
        foreach (AudioSource s in effects.Values)
        {
            s.Stop();
        }
    }

    //ɾ��ָ����Ч�����Ľڵ�
    public static void clear_effect(string url)
    {
        AudioSource audio_source = null;
        if (!effects.ContainsKey(url))//�ж��Ƿ��Ѿ�����Ч��������
        {
            return;//û�������Ч��ֱ�ӷ���
        }
        audio_source = effects[url];//�оͰ�audio_sourceֱ�Ӹ�ֵ��ȥ
        effects[url] = null;//ָ��audio_source������
        GameObject.Destroy(audio_source.gameObject);//ɾ��������ָ��audio_source����Ľڵ�
    }

    //�л���Ч��������
    public static void switch_effect()
    {
        // �л���������������״̬
        is_effect_mute = !is_effect_mute;

        //�ѵ�ǰ�Ƿ���д�뱾��
        int value = (is_effect_mute) ? 1 : 0;//boolת��int
        PlayerPrefs.SetInt("effect_mute", value);

        // ����������Ч��AudioSourceԪ��
        foreach (AudioSource s in effects.Values)
        {
            s.mute = is_effect_mute;//����Ϊ��ǰ��״̬
        }
    }

    //���ҵĽ���ľ�����ťҪ��ʾ��ʱ�򣬵�������ʾ�رգ����ǿ�ʼ״̬;
    public static bool effect_is_off()
    {
        return is_effect_mute;
    }
    //�Ż����Խӿ�
    public static void disable_over_audio()
    {
        //�����������ֱ�
        foreach (AudioSource s in musics.Values)
        {
            if (!s.isPlaying)//�ж��Ƿ��ڲ���
            {
                s.enabled = false;//���ڲ��ž�ֱ������
            }
        }

        //������Ч��
        foreach (AudioSource s in effects.Values)
        {
            if (!s.isPlaying)//�ж��Ƿ��ڲ���
            {
                s.enabled = false;//���ڲ��ž�ֱ������
            }
        }
    }
}


using System;

namespace CardioCore
{
    public sealed class TwinState
    {
        public float Bpm;
        public float Phase;
        public double RPeakT;
        public float RS1Ms;
        public float S1S2Ms;
        public string RhythmLabel;
        public float RhythmConf;
        public string MurmurLabel;
        public float MurmurConf;
        public bool LeadOff;
        public float[] Ecg = Array.Empty<float>();
        public float[] PcgEnv = Array.Empty<float>();

        public void CopyFrom(Wire w)
        {
            Bpm = w.bpm;
            Phase = w.phase;
            RPeakT = w.r_peak_t;
            RS1Ms = w.r_s1_ms;
            S1S2Ms = w.s1_s2_ms;
            RhythmLabel = w.rhythm_label;
            RhythmConf = w.rhythm_conf;
            MurmurLabel = w.murmur_label;
            MurmurConf = w.murmur_conf;
            LeadOff = w.lead_off;
            Ecg = w.ecg ?? Array.Empty<float>();
            PcgEnv = w.pcg_env ?? Array.Empty<float>();
        }

        [Serializable]
        public sealed class Wire
        {
            public float bpm;
            public float phase;
            public double r_peak_t;
            public float r_s1_ms;
            public float s1_s2_ms;
            public string rhythm_label;
            public float rhythm_conf;
            public string murmur_label;
            public float murmur_conf;
            public bool lead_off;
            public float[] ecg;
            public float[] pcg_env;
        }
    }
}
using UnityEngine;
using System.Collections;
using FixedPointy;

public interface IRecordedObject
{
    void ResetRecording();
    void Record();
    void SetLast(Fix time);
    void RecordToCache(Fix time);
    void CacheClear();
    int CacheSize();
    void SetFromCache();
}

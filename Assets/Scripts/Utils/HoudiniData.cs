using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoudiniEngineUnity;

public static class HoudiniData {
    public static Vector3[] ParseVector(HEU_OutputAttribute attr) {
        var vec = new Vector3[attr._count];
        for (int i = 0; i < attr._count; i++) {
            var j = i * attr._tupleSize;
            var val = Vector3.zero;
            switch (attr._type) {
                case HAPI_StorageType.HAPI_STORAGETYPE_INT:
                case HAPI_StorageType.HAPI_STORAGETYPE_INT64:
                case HAPI_StorageType.HAPI_STORAGETYPE_UINT8:
                case HAPI_StorageType.HAPI_STORAGETYPE_INT8:
                case HAPI_StorageType.HAPI_STORAGETYPE_INT16:
                case HAPI_StorageType.HAPI_STORAGETYPE_INT_ARRAY:
                case HAPI_StorageType.HAPI_STORAGETYPE_INT64_ARRAY:
                case HAPI_StorageType.HAPI_STORAGETYPE_UINT8_ARRAY:
                case HAPI_StorageType.HAPI_STORAGETYPE_INT8_ARRAY:
                case HAPI_StorageType.HAPI_STORAGETYPE_INT16_ARRAY:
                    val.x = attr._intValues[j];
                    if (attr._tupleSize > 1) val.y = attr._intValues[j+1];
                    if (attr._tupleSize > 2) val.z = attr._intValues[j+2];
                    break;
                case HAPI_StorageType.HAPI_STORAGETYPE_FLOAT:
                case HAPI_StorageType.HAPI_STORAGETYPE_FLOAT64:
                case HAPI_StorageType.HAPI_STORAGETYPE_FLOAT_ARRAY:
                case HAPI_StorageType.HAPI_STORAGETYPE_FLOAT64_ARRAY:
                    val.x = attr._floatValues[j];
                    if (attr._tupleSize > 1) val.y = attr._floatValues[j+1];
                    if (attr._tupleSize > 2) val.z = attr._floatValues[j+2];
                    break;
            }
            val.x *= -1; // inverse X
            vec[i] = val;
        }

        return vec;
    }
}

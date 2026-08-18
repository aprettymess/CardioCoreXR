#include "pch-cpp.hpp"






struct ExportContext_t91248C8B322E6B6FCF18BEC292C533E573056D07;
struct GLTFExportPlugin_t9CD417E2451F6F5380052BE84CFBFFB9FC0C59A6;
struct GLTFExportPluginContext_t8C6EC453DF190D7EE2AEE0B89FE7553F91AA7C43;
struct GLTFSettings_tA61B9FAC06F115923FFB98D549BA7C467B33C952;
struct ILogger_tD1F573C6DC829FBA987FA1EBA0A5FA64E0C2BC42;
struct String_t;
struct VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8;
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;
struct AfterMaterialExportDelegate_t5DEA3BABA1473661425A2A70967E778839DEE5F6;
struct AfterMeshExportDelegate_tD17E279DEF2276BF5C58DBD9F3662A7BF52A6743;
struct AfterNodeExportDelegate_tD87E44A115D574BACC84A57B7F390E4EC4C117C2;
struct AfterPrimitiveExportDelegate_t1173AE0DEF8E908A1B1ECFE089E95C46B25A7742;
struct AfterSceneExportDelegate_t30E3645A22D0DC20CD7AC35501AD33D3CAA5E42A;
struct AfterTextureExportDelegate_t281E07AEE71651B3CA12F218AA09175A771DBD11;
struct BeforeMaterialExportDelegate_t6A400E9D47A4F15D87D712660DF1D262BE89978B;
struct BeforeSceneExportDelegate_t46F2E55C895D42A4500F5B14D20C79C30824F3B2;
struct BeforeTextureExportDelegate_t19D640C81E7B28C04A4538B20430C728F1B6AE15;
struct RetrieveTexturePathDelegate_tB2FB2B79A83A98694C435FE8859D27CADF8AEF8E;

IL2CPP_EXTERN_C RuntimeClass* Debug_t8394C7EEAECA3689C2C9B9DE9C7166D73596276F_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteral6C1E5E213D9E08550846E8242B5504A205317E9F;
IL2CPP_EXTERN_C String_t* _stringLiteralC6A440C23497F6AADE7BE9842DA26743ECB4ADE9;
IL2CPP_EXTERN_C String_t* _stringLiteralE8CA43C8EE3DB13E0ECC8CE07F31F7CBA95143FA;


IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
struct U3CModuleU3E_t83A8D773D72500158025BE3EBADEDC28BD14A45E 
{
};
struct GLTFExportPluginContext_t8C6EC453DF190D7EE2AEE0B89FE7553F91AA7C43  : public RuntimeObject
{
};
struct String_t  : public RuntimeObject
{
	int32_t ____stringLength;
	Il2CppChar ____firstChar;
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	bool ___m_value;
};
struct IntPtr_t 
{
	void* ___m_value;
};
struct LayerMask_t97CB6BDADEDC3D6423C7BCFEA7F86DA2EC6241DB 
{
	int32_t ___m_Mask;
};
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};
struct ExportContext_t91248C8B322E6B6FCF18BEC292C533E573056D07  : public RuntimeObject
{
	bool ___TreatEmptyRootAsScene;
	bool ___MergeClipsWithMatchingNames;
	LayerMask_t97CB6BDADEDC3D6423C7BCFEA7F86DA2EC6241DB ___ExportLayers;
	RuntimeObject* ___logger;
	GLTFSettings_tA61B9FAC06F115923FFB98D549BA7C467B33C952* ___settings;
	RetrieveTexturePathDelegate_tB2FB2B79A83A98694C435FE8859D27CADF8AEF8E* ___TexturePathRetriever;
	AfterSceneExportDelegate_t30E3645A22D0DC20CD7AC35501AD33D3CAA5E42A* ___AfterSceneExport;
	BeforeSceneExportDelegate_t46F2E55C895D42A4500F5B14D20C79C30824F3B2* ___BeforeSceneExport;
	AfterNodeExportDelegate_tD87E44A115D574BACC84A57B7F390E4EC4C117C2* ___AfterNodeExport;
	BeforeMaterialExportDelegate_t6A400E9D47A4F15D87D712660DF1D262BE89978B* ___BeforeMaterialExport;
	AfterMaterialExportDelegate_t5DEA3BABA1473661425A2A70967E778839DEE5F6* ___AfterMaterialExport;
	BeforeTextureExportDelegate_t19D640C81E7B28C04A4538B20430C728F1B6AE15* ___BeforeTextureExport;
	AfterTextureExportDelegate_t281E07AEE71651B3CA12F218AA09175A771DBD11* ___AfterTextureExport;
	AfterPrimitiveExportDelegate_t1173AE0DEF8E908A1B1ECFE089E95C46B25A7742* ___AfterPrimitiveExport;
	AfterMeshExportDelegate_tD17E279DEF2276BF5C58DBD9F3662A7BF52A6743* ___AfterMeshExport;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C  : public RuntimeObject
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr;
};
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
	intptr_t ___m_CachedPtr;
};
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A_marshaled_pinvoke : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
};
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A_marshaled_com : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
};
struct GLTFPlugin_t96DF133E35E8D32BE46450289CAC0153A379A953  : public ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A
{
	bool ___enabled;
};
struct GLTFExportPlugin_t9CD417E2451F6F5380052BE84CFBFFB9FC0C59A6  : public GLTFPlugin_t96DF133E35E8D32BE46450289CAC0153A379A953
{
};
struct VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8  : public GLTFExportPlugin_t9CD417E2451F6F5380052BE84CFBFFB9FC0C59A6
{
	bool ___cleanUpAndOptimizeExportedGraph;
};
struct String_t_StaticFields
{
	String_t* ___Empty;
};
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	String_t* ___TrueString;
	String_t* ___FalseString;
};
#ifdef __clang__
#pragma clang diagnostic pop
#endif



IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Debug_LogWarning_m33EF1B897E0C7C6FF538989610BFAFFEF4628CA9 (RuntimeObject* ___0_message, const RuntimeMethod* method) ;
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GLTFExportPlugin__ctor_m41AFD4FDF9B4B9A1BE87FA34342019697087BD14 (GLTFExportPlugin_t9CD417E2451F6F5380052BE84CFBFFB9FC0C59A6* __this, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Method Definition Index: 101700
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool VisualScriptingExportPlugin_get_Enabled_mBF3CADD354A69DFB1B53F48A936D9832DAA00CCA (VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/org.khronos.unitygltf@f0295f76355a/Runtime/Scripts/Interactivity/VisualScripting/Plugin/VisualScriptingExportPlugin.cs:22>
		return (bool)0;
	}
}
// Method Definition Index: 101701
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool VisualScriptingExportPlugin_get_EnabledByDefault_m3F758A1C8652459F3FCEF2D3015434818E63A02F (VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/org.khronos.unitygltf@f0295f76355a/Runtime/Scripts/Interactivity/VisualScripting/Plugin/VisualScriptingExportPlugin.cs:25>
		return (bool)1;
	}
}
// Method Definition Index: 101702
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* VisualScriptingExportPlugin_get_DisplayName_m5220F30BFD682E6CBF9C9385017255992FE6910F (VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC6A440C23497F6AADE7BE9842DA26743ECB4ADE9);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/org.khronos.unitygltf@f0295f76355a/Runtime/Scripts/Interactivity/VisualScripting/Plugin/VisualScriptingExportPlugin.cs:31>
		return _stringLiteralC6A440C23497F6AADE7BE9842DA26743ECB4ADE9;
	}
}
// Method Definition Index: 101703
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* VisualScriptingExportPlugin_get_Description_mCD616B0030F25D0241EDD5B6BDA3284173F87B19 (VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE8CA43C8EE3DB13E0ECC8CE07F31F7CBA95143FA);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/org.khronos.unitygltf@f0295f76355a/Runtime/Scripts/Interactivity/VisualScripting/Plugin/VisualScriptingExportPlugin.cs:32>
		return _stringLiteralE8CA43C8EE3DB13E0ECC8CE07F31F7CBA95143FA;
	}
}
// Method Definition Index: 101704
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR GLTFExportPluginContext_t8C6EC453DF190D7EE2AEE0B89FE7553F91AA7C43* VisualScriptingExportPlugin_CreateInstance_m7EAFA57FAA65A548FD3EDC64C86B1180FA6F78E9 (VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8* __this, ExportContext_t91248C8B322E6B6FCF18BEC292C533E573056D07* ___0_context, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Debug_t8394C7EEAECA3689C2C9B9DE9C7166D73596276F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral6C1E5E213D9E08550846E8242B5504A205317E9F);
		s_Il2CppMethodInitialized = true;
	}
	{
		//<source_info:./Library/PackageCache/org.khronos.unitygltf@f0295f76355a/Runtime/Scripts/Interactivity/VisualScripting/Plugin/VisualScriptingExportPlugin.cs:46>
		il2cpp_codegen_runtime_class_init_inline(Debug_t8394C7EEAECA3689C2C9B9DE9C7166D73596276F_il2cpp_TypeInfo_var);
		Debug_LogWarning_m33EF1B897E0C7C6FF538989610BFAFFEF4628CA9(_stringLiteral6C1E5E213D9E08550846E8242B5504A205317E9F, NULL);
		//<source_info:./Library/PackageCache/org.khronos.unitygltf@f0295f76355a/Runtime/Scripts/Interactivity/VisualScripting/Plugin/VisualScriptingExportPlugin.cs:47>
		return (GLTFExportPluginContext_t8C6EC453DF190D7EE2AEE0B89FE7553F91AA7C43*)NULL;
	}
}
// Method Definition Index: 101705
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void VisualScriptingExportPlugin__ctor_mA560EE642BDD3EC5BCD3AF146BE64FAEA48655CB (VisualScriptingExportPlugin_t158FB212CE648BDCB330EFC9A10537D57D2683E8* __this, const RuntimeMethod* method) 
{
	{
		//<source_info:./Library/PackageCache/org.khronos.unitygltf@f0295f76355a/Runtime/Scripts/Interactivity/VisualScripting/Plugin/VisualScriptingExportPlugin.cs:36>
		__this->___cleanUpAndOptimizeExportedGraph = (bool)1;
		GLTFExportPlugin__ctor_m41AFD4FDF9B4B9A1BE87FA34342019697087BD14(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif

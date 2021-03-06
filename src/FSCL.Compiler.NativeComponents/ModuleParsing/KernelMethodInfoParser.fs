﻿namespace FSCL.Compiler.ModuleParsing

open FSCL.Compiler
open FSCL.Compiler.Util
open System.Collections.Generic
open System.Reflection
open Microsoft.FSharp.Quotations
open FSCL.Language
open FSCL

open QuotationAnalysis.FunctionsManipulation
open QuotationAnalysis.KernelParsing
open QuotationAnalysis.MetadataExtraction

[<StepProcessor("FSCL_METHOD_INFO_PARSING_PROCESSOR", "FSCL_MODULE_PARSING_STEP")>]
type KernelMethodInfoParser() =      
    inherit ModuleParsingProcessor() 
        
    override this.Run((mi, envBuilder), s, opts) =
        let step = s :?> ModuleParsingStep
        if (mi :? MethodInfo) then
            match GetKernelFromMethodInfo(mi :?> MethodInfo) with
            | Some(obv, ob, mi, paramInfo, paramVars, b, kMeta, rMeta, pMeta) -> 
                // Filter and finalize metadata
                let finalMeta = step.ProcessMeta(kMeta, rMeta, pMeta, null, opts)

                // Create singleton kernel call graph
                let kernelModule = new KernelModule(obv, None, 
                                                    new KernelInfo(mi.Name, Some(mi), paramVars, mi.ReturnType,
                                                                   new List<Var>(), new List<Expr>(), 
                                                                   None, b, finalMeta))
                
                // Create node
                let node = new KFGKernelNode(kernelModule)                
                Some(node :> IKFGNode)  
            | _ ->
                None
        else
            None
            
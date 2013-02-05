﻿namespace FSCL.Compiler.Processors

open FSCL.Compiler
open System.Collections.Generic
open System.Reflection
open Microsoft.FSharp.Quotations

[<StepProcessor("FSCL_METHOD_INFO_PARSING_PROCESSOR", "FSCL_MODULE_PARSING_STEP")>]
type KernelMethodInfoParser() =      
    let rec GetKernelFromName(mi, k:ModuleParsingStep) =       
        match mi with
        | DerivedPatterns.MethodWithReflectedDefinition(b) ->
            Some(mi, b)
        | _ ->
            None
        
    interface ModuleParsingProcessor with
        member this.Handle(mi, engine:ModuleParsingStep) =
            if (mi.GetType() = typeof<MethodInfo>) then
                match GetKernelFromName(mi :?> MethodInfo, engine) with
                | Some(mi, b) -> 
                    let km = new KernelModule()
                    km.Source <- new KernelInfo(mi, b)
                    Some(km)
                | _ ->
                    None
            else
                None
            
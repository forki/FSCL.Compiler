﻿namespace FSCL.Compiler.Processors

open FSCL.Compiler
open System.Collections.Generic
open System.Reflection
open Microsoft.FSharp.Quotations

[<StepProcessor("FSCL_SEQUENTIAL_PRETTY_PRINTING_PROCESSOR", "FSCL_FUNCTION_PRETTY_PRINTING_STEP")>]
type SequentialPrinter() =   
    interface FunctionBodyPrettyPrintingProcessor with
        member this.Handle(expr, engine:FunctionPrettyPrintingStep) =
            match expr with
            | Patterns.Sequential(e1, e2) ->
                Some(engine.Continue(e1) + "\n" + engine.Continue(e2))
            | _ ->
                None
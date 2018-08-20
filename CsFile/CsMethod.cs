using System;
using System.Collections.Generic;

namespace CsFile
{
    public class CsMethod
    {
        public AccessScope Accessor { get; set; }
        public bool IsStatic { get; set; }
        public String Name { get; set; }
        public bool IsReadOnly { get; set; }
        public List<MethodParameter> Parameters { get; set; }
        public MethodParameter ReturnValue { get; set; }
        List<ScriptBlock> Blocks { get; set; }
    }
    public class MethodParameter
    {
        public ParameterDirection Direction { get; set; }
        public String Name { get; set; }
        public Type ParameterType { get; set; }
    }

    public enum ParameterDirection
    {
        Out, In, Void
    }
    public class ScriptBlock
    {
        public BlockType ScriptBlockType { get; set; }
        public ScopeContext Context { get; set; }
        public ScriptBlock(ScopeContext context)
        {
            this.Context = context;
        }
        public string Content { get; set; }
        public List<String> Lines { get; set; }
    }

    public enum BlockType
    {
        ANONYMOUS,IF,IF_ELSE,SWITCH,WHILE,DO_WHILE,USING,LOCK,METHOD
    }

    public class ScopeContext
    {

    }

    public class SentenceSplitor
    {

    }
    public class LineAnalysis
    {

    }
}
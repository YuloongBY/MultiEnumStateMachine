/**
 * The MIT License (MIT)
 *
 * Copyright (c) 2022 YuloongBY - Github: github.com/YuloongBY
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of
 * this software and associated documentation files (the "Software"), to deal in
 * the Software without restriction, including without limitation the rights to
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
 * the Software, and to permit persons to whom the Software is furnished to do so,
 * subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///　インデックスベースクラス
/// </summary>
public class IndexBase : IIdxConverterIF
{
    //使ってないインデックス
    private int emptyIdx_ = 0; 

    /// <summary>
    /// 使ってないインデックスを取得し、更新する
    /// </summary>
    public int I_GetAndUpdateEmptyIdx()
    {
        int emptyIdx = emptyIdx_;
        emptyIdx_++;
        return emptyIdx;
    }
}

/// <summary>
/// インデックス転換処理
/// </summary>
public class IdxConverter<CONTENT> where CONTENT : Enum
{
    //オーナー
    public IIdxConverterIF owner_ = null;
        
    //インデックス
    private Dictionary<CONTENT,int> contentIdx_ = new Dictionary<CONTENT, int>();
    //逆インデックス
    private Dictionary<int,CONTENT> idxContent_ = new Dictionary<int, CONTENT>();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public IdxConverter( IIdxConverterIF _owner ){ owner_ = _owner;} 
    ~IdxConverter(){}

    /// <summary>
    /// インデックスからコンテントを取得
    /// </summary>
    public bool GetContentByIdx( out CONTENT _content , int _idx )
    {
        if( idxContent_.ContainsKey( _idx ))
        {
            _content = idxContent_[ _idx ];
            return true;
        }
        _content = default( CONTENT );
        return false;
    }

    /// <summary>
    /// コンテントからインデックスを取得
    /// </summary>
    public int GetIdxByContent( CONTENT _state )
    {
        if( contentIdx_.ContainsKey( _state ))
        {
            return contentIdx_[ _state ];
        }
        Debug.LogWarning( $"存在しません = {_state}" );
        return -1;
    }

    /// <summary>
    /// コンテントを登録し、コンテントのインデックスを返す
    /// </summary>
    public int RegisterAndReturnIdx( CONTENT _content )
    {
        if( !contentIdx_.ContainsKey( _content ))
        {
            int emptyIdx = owner_.I_GetAndUpdateEmptyIdx();
            contentIdx_.Add( _content , emptyIdx );
            idxContent_.Add( emptyIdx , _content );
            return emptyIdx;
        }
        return contentIdx_[ _content ];
    }

    /// <summary>
    /// 辞書から削除
    /// </summary>
    public void RemoveContent( CONTENT _content )
    {
        if( contentIdx_.ContainsKey( _content ))
        {
            int idx = GetIdxByContent( _content );
            contentIdx_.Remove( _content );
            idxContent_.Remove( idx );
        }
    }

    /// <summary>
    /// 辞書から削除
    /// </summary>
    public void RemoveContent( int _idx )
    {
        if( idxContent_.ContainsKey( _idx ))
        {
            CONTENT content;
            bool isSuccess = GetContentByIdx( out content , _idx );
            if( isSuccess )
            {
                idxContent_.Remove(_idx);
                contentIdx_.Remove(content);
            }
        }
    }
}

/// <summary>
/// インデックス転換処理インタフェース
/// </summary>
public interface IIdxConverterIF
{
    /// <summary>
    /// 使ってないインデックスを取得し、更新する
    /// </summary>
    int I_GetAndUpdateEmptyIdx();
}


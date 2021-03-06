/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

import React from 'react';
import styled from 'styled-components';
import { HTMLAttributes } from 'react';
import { disabledTextColor, textColor } from '../styleVariables';

const CodePreviewWrapper = styled.div`
  padding: 10px;
`;

const CodePreviewBox = styled.div`
  border-radius: 5px;
  font-family: 'Courier New', Courier, monospace;
  background: rgba(0, 0, 0, 0.05);
  outline: none;
  font-size: 15px;
  padding: 10px;
  border: none;
  resize: none;
  box-shadow: inset rgba(0, 0, 0, 0.5) 0 0 10px -5px;
  white-space: pre-wrap;
  overflow: auto;
  position: relative;
`;

const Code = styled.div`
  color: ${textColor};
  line-height: 20px;
  font-size: 15px;
`;

const CodeRuler = styled.div<{ $maxLineLength: number }>`
  top: 0;
  left: 0;
  width: calc(10px + ${({ $maxLineLength }) => $maxLineLength}ch);
  border-right: 1px solid ${disabledTextColor};
  position: absolute;
  height: 100%;
  pointer-events: none;
`;

type CodePreviewProps = {
  maxLineLength?: number;
} & HTMLAttributes<HTMLSpanElement>;

const CodePreview = ({ maxLineLength, children }: CodePreviewProps) => (
  <CodePreviewWrapper>
    <CodePreviewBox>
      <Code>{children}</Code>
      {maxLineLength ? <CodeRuler $maxLineLength={maxLineLength} /> : null}
    </CodePreviewBox>
  </CodePreviewWrapper>
);

export default CodePreview;
